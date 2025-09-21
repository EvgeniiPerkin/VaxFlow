using DocumentFormat.OpenXml.EMMA;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
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
        public async Task<ObservableCollection<PartySummaryModel>> GetAvailablePartiesAsync(SqliteConnection connection)
        {
            ObservableCollection<PartySummaryModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT id, count, dt_create, party_name, vaccine_id, vaccine_name, vaccine_version, vaccine_version_id
                FROM party_summary
                WHERE count > 0;";
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Count = reader.GetInt32(1),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 2),
                    PartyName = reader.GetString(3),
                    VaccineId = reader.GetInt32(4),
                    VaccineName = reader.GetString(5),
                    VaccineVersion = reader.GetString(6),
                    VaccineVersionId = reader.GetInt32(7)
                });
            }
            return items;
        }
        public async Task<ObservableCollection<PartySummaryModel>> FindByPartyNameAsync(SqliteConnection connection, string partyName)
        {
            var cleanPartyName = partyName
                .Replace("%", "\\%")
                .Replace("_", "\\_")
                .Replace("[", "\\[")
                .Replace("]", "\\]");

            ObservableCollection<PartySummaryModel> items = [];

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT id, count, dt_create, party_name, vaccine_id, vaccine_name, vaccine_version, vaccine_version_id
                FROM party_summary
                WHERE party_name LIKE @party_name;";
            cmd.Parameters.AddWithValue("@party_name", $"%{cleanPartyName.Trim()}%");

            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Count = reader.GetInt32(1),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 2),
                    PartyName = reader.GetString(3),
                    VaccineId = reader.GetInt32(4),
                    VaccineName = reader.GetString(5),
                    VaccineVersion = reader.GetString(6),
                    VaccineVersionId = reader.GetInt32(7)
                });
            }
            return items;
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
