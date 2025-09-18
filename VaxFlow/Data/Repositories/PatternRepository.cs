using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class PatternRepository
    {
        public async Task<ObservableCollection<PatternModel>> GetAllAsync(SqliteConnection connection)
        {
            ObservableCollection<PatternModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, desc FROM patterns;";
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Desc = reader.GetString(1)
                });
            }
            return items;
        }

        public async Task<int> CreateAsync(SqliteConnection connection, PatternModel value)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO patterns (desc)
                VALUES (@desc);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@desc", value.Desc);
            var id = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            value.Id = Convert.ToInt32(id);
            return value.Id;
        }

        public async Task<int> DeleteAsync(SqliteConnection connection, PatternModel value)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM patterns WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", value.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(SqliteConnection connection, PatternModel value)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE patterns
                SET desc=@desc
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@desc", value.Desc);
            cmd.Parameters.AddWithValue("@id", value.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
