using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class DiseaseRepository
    {
        public async Task<int> CreateAsync(SqliteConnection connection, DiseaseModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO diseases (desc)
                VALUES (@desc);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@desc", model.Desc);
            var id = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            model.Id = Convert.ToInt32(id);
            return model.Id;
        }
        public async Task<ObservableCollection<DiseaseModel>> GetAllAsync(SqliteConnection connection)
        {
            ObservableCollection<DiseaseModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, desc FROM diseases;";
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
        public async Task<int> UpdateAsync(SqliteConnection connection, DiseaseModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE diseases
                SET desc=@desc
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@desc", model.Desc);
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
