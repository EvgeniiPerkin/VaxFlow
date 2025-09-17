using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class VaccineVersionRepository
    {
        public async Task<int> CreateAsync(SqliteConnection connection, VaccineVersionModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO vaccine_versions (version)
                VALUES (@version);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@version", model.Version);
            var id = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            model.Id = Convert.ToInt32(id);
            return model.Id;
        }
        public async Task<ObservableCollection<VaccineVersionModel>> GetAllAsync(SqliteConnection connection)
        {
            ObservableCollection<VaccineVersionModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, version FROM vaccine_versions;";
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Version = reader.GetString(1)
                });
            }
            return items;
        }
        public async Task<int> UpdateAsync(SqliteConnection connection, VaccineVersionModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE vaccine_versions
                SET version=@version
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@version", model.Version);
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
