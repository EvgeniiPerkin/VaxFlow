using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VaxFlow.Services;

namespace VaxFlow.Data
{
    public class DbContext
    {
        public DbContext(IAppConfiguration configuration, IMyLogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        #region Properties
        private readonly IAppConfiguration configuration;
        private readonly IMyLogger logger;
        #endregion

        public async Task<int> SetupAsync()
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                List<Migration> filesMigrations = await Task.Run(() => GetMigrationsFromFiles()).ConfigureAwait(false);
                if (filesMigrations.Count == 0)
                {
                    throw new Exception("Migration files are missing!");
                }

                using var enableFkCommand = connection.CreateCommand();
                enableFkCommand.CommandText = "PRAGMA foreign_keys = ON;";
                await enableFkCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

                using var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS [migrations] (
                        id INTEGER NOT NULL,
                        version INTEGER NOT NULL,
                        sql TEXT NOT NULL,
                        PRIMARY KEY(id AUTOINCREMENT)
                    );
                ";
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                List<Migration> dbMigrations = await GetMigrationsFromDbAsync(connection);

                foreach (var mFl in filesMigrations)
                {
                    foreach (var mDb in dbMigrations)
                    {
                        if (mDb.Version.Equals(mFl.Version))
                        {
                            if (!mDb.Sql.Equals(mFl.Sql))
                                throw new Exception("The migration file has been changed!");
                            
                            mFl.IsFound = true;
                            break;
                        }                        
                    }
                }
                foreach (var mFl in filesMigrations)
                {
                    if (!mFl.IsFound)
                    {
                        await ApplyMigrationAsync(connection, mFl);
                        await AddMigragionInDb(connection, mFl);
                    }
                }

                transaction.Commit();
                return 0;
            }
            catch(Exception ex) 
            {
                transaction.Rollback();
                logger.Error(ex, "Error during database initialization.");
                return -1;
            }
        }
        private static async Task<List<Migration>> GetMigrationsFromDbAsync(SqliteConnection connection)
        {
            List<Migration> migrations = [];

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT version, sql FROM [migrations] ORDER BY version;";
            
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                migrations.Add(new Migration()
                {
                    Version = reader.GetInt32(0),
                    Sql = reader.GetString(1)
                });
            }

            return migrations;
        }
        private static List<Migration> GetMigrationsFromFiles()
        {
            List<Migration> migrations = [];
            DirectoryInfo directory = new(Path.Combine(Directory.GetCurrentDirectory(), "Migrations"));
            FileInfo[] files = directory.GetFiles("*.sql");
            var sortedByName = files.OrderBy(f => f.Name);

            foreach (FileInfo file in sortedByName)
            {
                if (file.Name.Substring(0, 1) != "V") throw new Exception("Incorrect naming of the migration file.");

                if (int.TryParse(file.Name.AsSpan(1, 1), out int version))
                {
                    using StreamReader reader = file.OpenText();
                    string content = reader.ReadToEnd();
                    migrations.Add(new()
                    {
                        Version = version,
                        Sql = content
                    });
                }
                else
                {
                    throw new Exception("Incorrect naming of the migration file.");
                }
            }

            return migrations;
        }
        private static async Task ApplyMigrationAsync(SqliteConnection connection, Migration migration)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = migration.Sql;
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        private static async Task AddMigragionInDb(SqliteConnection connection, Migration mFl)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO [migrations] (version, sql) 
                VALUES (@version, @sql)";
            var versionParam = cmd.Parameters.Add("@version", SqliteType.Integer);
            var sqlParam = cmd.Parameters.Add("@sql", SqliteType.Text);
            versionParam.Value = mFl.Version;
            sqlParam.Value = mFl.Sql;
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }

    public record Migration
    {
        public int Version { get; set; }
        public string Sql { get; set; } = string.Empty;
        public bool IsFound { get; set; } = false;
    }
}
