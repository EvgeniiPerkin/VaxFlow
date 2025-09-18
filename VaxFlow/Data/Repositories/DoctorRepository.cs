using DocumentFormat.OpenXml.EMMA;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class DoctorRepository
    {
        public async Task<int> CreateAsync(SqliteConnection connection, DoctorModel doctor)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO doctors (last_name, first_name, patronymic, name_suffix, is_dismissed)
                VALUES (@last_name, @first_name, @patronymic, @name_suffix, @is_dismissed);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@last_name", doctor.LastName);
            cmd.Parameters.AddWithValue("@first_name", doctor.FirstName);
            cmd.Parameters.AddWithValue("@patronymic", SqliteHelper.FromStringNull(doctor.Patronymic));
            cmd.Parameters.AddWithValue("@name_suffix", SqliteHelper.FromStringNull(doctor.NameSuffix));
            cmd.Parameters.AddWithValue("@is_dismissed", SqliteHelper.FromBoolean(doctor.IsDismissed));
            var id = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            doctor.Id = Convert.ToInt32(id);
            return doctor.Id;
        }
        public async Task<ObservableCollection<DoctorModel>> GetAllAsync(SqliteConnection connection)
        {
            ObservableCollection<DoctorModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, last_name, first_name, patronymic, name_suffix, is_dismissed FROM doctors;";
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    LastName = reader.GetString(1),
                    FirstName = reader.GetString(2),
                    Patronymic = SqliteHelper.GetStringNull(reader, 3),
                    NameSuffix = SqliteHelper.GetStringNull(reader, 4),
                    IsDismissed = SqliteHelper.GetBoolean(reader, 5)
                });
            }
            return items;
        }
        public async Task<int> UpdateAsync(SqliteConnection connection, DoctorModel doctor)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE doctors
                SET last_name=@last_name, first_name=@first_name, patronymic=@patronymic, name_suffix=@name_suffix, is_dismissed=@is_dismissed
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@last_name", doctor.LastName);
            cmd.Parameters.AddWithValue("@first_name", doctor.FirstName);
            cmd.Parameters.AddWithValue("@patronymic", SqliteHelper.FromStringNull(doctor.Patronymic));
            cmd.Parameters.AddWithValue("@name_suffix", SqliteHelper.FromStringNull(doctor.NameSuffix));
            cmd.Parameters.AddWithValue("@is_dismissed", SqliteHelper.FromBoolean(doctor.IsDismissed));
            cmd.Parameters.AddWithValue("@id", doctor.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteAsync(SqliteConnection connection, DoctorModel doctor)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM doctors WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", doctor.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
