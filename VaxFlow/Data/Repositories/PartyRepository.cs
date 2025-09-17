using Microsoft.Data.Sqlite;
using System;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class PartyRepository
    {
        public async Task<int> CreateAsync(SqliteConnection connection, PartyModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO parties (vaccine_id, vaccine_version_id, party_name, count, dt_create)
                VALUES (@vaccine_id, @vaccine_version_id, @party_name, @count, @dt_create);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@vaccine_id", model.VaccineId);
            cmd.Parameters.AddWithValue("@vaccine_version_id", model.VaccineVersionId);
            cmd.Parameters.AddWithValue("@party_name", model.PartyName);
            cmd.Parameters.AddWithValue("@count", model.Count);
            cmd.Parameters.AddWithValue("@dt_create", SqliteHelper.FromDateTime(model.DateTimeCreate));
            var id = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            model.Id = Convert.ToInt32(id);
            return model.Id;
        }
        public async Task<int> UpdateCountAsync(SqliteConnection connection, PartyModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE parties
                SET count=@count
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@count", model.Count);
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        public async Task<int> DeleteAsync(SqliteConnection connection, PartyModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM parties WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
