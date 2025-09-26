using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class VaccineRepository
    {
        public async Task<int> CreateAsync(SqliteConnection connection, VaccineModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO vaccines (disease_id, vaccine_version_id, vaccine_name, count, dt_create, expiration_date, series)
                VALUES (@disease_id, @vaccine_version_id, @vaccine_name, @count, @dt_create, @expiration_date, @series);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@disease_id", model.DiseaseId);
            cmd.Parameters.AddWithValue("@vaccine_version_id", model.VaccineVersionId);
            cmd.Parameters.AddWithValue("@vaccine_name", model.VaccineName);
            cmd.Parameters.AddWithValue("@count", model.Count);
            cmd.Parameters.AddWithValue("@dt_create", SqliteHelper.FromDateTime(model.DateTimeCreate));
            cmd.Parameters.AddWithValue("@expiration_date", SqliteHelper.FromDate(model.ExpirationDate));
            cmd.Parameters.AddWithValue("@series", model.Series);
            var id = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            model.Id = Convert.ToInt32(id);
            return model.Id;
        }
        public async Task<ObservableCollection<VaccineSummaryModel>> GetAvailableVaccinesAsync(SqliteConnection connection)
        {
            ObservableCollection<VaccineSummaryModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT id, count, dt_create, vaccine_name, disease_id, vaccine_name, vaccine_version, vaccine_version_id, expiration_date, series
                FROM vaccine_summary
                WHERE count > 0;";
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Count = reader.GetInt32(1),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 2),
                    VaccineName = reader.GetString(3),
                    DiseaseId = reader.GetInt32(4),
                    DiseaseName = reader.GetString(5),
                    VaccineVersion = reader.GetString(6),
                    VaccineVersionId = reader.GetInt32(7),
                    ExpirationDate = SqliteHelper.GetDate(reader, 8),
                    Series = reader.GetString(9)
                });
            }
            return items;
        }
        public async Task<ObservableCollection<VaccineSummaryModel>> FindByVaccineNameAsync(SqliteConnection connection, string vaccineName)
        {
            var cleanVaccineName = vaccineName
                .Replace("%", "\\%")
                .Replace("_", "\\_")
                .Replace("[", "\\[")
                .Replace("]", "\\]");

            ObservableCollection<VaccineSummaryModel> items = [];

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT id, count, dt_create, vaccine_name, disease_id, vaccine_name, vaccine_version, vaccine_version_id, expiration_date, series
                FROM vaccine_summary
                WHERE vaccine_name LIKE @vaccine_name;";
            cmd.Parameters.AddWithValue("@vaccine_name", $"%{cleanVaccineName.Trim()}%");

            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Count = reader.GetInt32(1),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 2),
                    VaccineName = reader.GetString(3),
                    DiseaseId = reader.GetInt32(4),
                    DiseaseName = reader.GetString(5),
                    VaccineVersion = reader.GetString(6),
                    VaccineVersionId = reader.GetInt32(7),
                    ExpirationDate = SqliteHelper.GetDate(reader, 8),
                    Series = reader.GetString(9)
                });
            }
            return items;
        }
        public async Task<int> UpdateCountAsync(SqliteConnection connection, VaccineModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE vaccines
                SET count=@count
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@count", model.Count);
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        public async Task<int> DeleteAsync(SqliteConnection connection, VaccineModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM vaccines WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        public async Task<int> UpdateAsync(SqliteConnection connection, VaccineModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE vaccines
                SET disease_id=@disease_id, vaccine_version_id=@vaccine_version_id,
                    vaccine_name=@vaccine_name, count=@count, dt_create=@dt_create,
                    expiration_date=@expiration_date, series=@series
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@disease_id", model.DiseaseId);
            cmd.Parameters.AddWithValue("@vaccine_version_id", model.VaccineVersionId);
            cmd.Parameters.AddWithValue("@vaccine_name", model.VaccineName);
            cmd.Parameters.AddWithValue("@count", model.Count);
            cmd.Parameters.AddWithValue("@dt_create", SqliteHelper.FromDateTime(model.DateTimeCreate));
            cmd.Parameters.AddWithValue("@expiration_date", SqliteHelper.FromDate(model.ExpirationDate));
            cmd.Parameters.AddWithValue("@series", model.VaccineName);
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
