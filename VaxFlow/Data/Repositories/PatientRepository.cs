using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class PatientRepository
    {
        public async Task<int> CreateAsync(SqliteConnection connection, PatientModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO patients
                    (last_name, first_name, patronymic, name_suffix, birthday, registration_address, 
                        dt_create, policy_number, working_position, job_category_id)
                VALUES 
                    (@last_name, @first_name, @patronymic, @name_suffix, @birthday, @registration_address,
                        @dt_create, @policy_number, @working_position, @job_category_id);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@last_name", model.LastName);
            cmd.Parameters.AddWithValue("@first_name", model.FirstName);
            cmd.Parameters.AddWithValue("@patronymic", SqliteHelper.FromStringNull(model.Patronymic));
            cmd.Parameters.AddWithValue("@name_suffix", SqliteHelper.FromStringNull(model.NameSuffix));
            cmd.Parameters.AddWithValue("@birthday", SqliteHelper.FromDate(model.Birthday));
            cmd.Parameters.AddWithValue("@registration_address", model.RegistrationAddress);
            cmd.Parameters.AddWithValue("@dt_create", SqliteHelper.FromDateTime(model.DateTimeCreate));
            cmd.Parameters.AddWithValue("@policy_number", model.PolicyNumber);
            cmd.Parameters.AddWithValue("@working_position", model.WorkingPosition);
            cmd.Parameters.AddWithValue("@job_category_id", model.JobCategoryId);
            var id = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            model.Id = Convert.ToInt32(id);
            return model.Id;
        }
        public async Task<ObservableCollection<PatientSummaryModel>> FilteredByDateCreateAsync(SqliteConnection connection, DateTime from, DateTime? to = null)
        {
            ObservableCollection<PatientSummaryModel> items = [];

            using var cmd = connection.CreateCommand();
            if (to == null)
            {
                cmd.CommandText = @"
                SELECT patient_initials, id, last_name, first_name, patronymic, name_suffix, policy_number, dt_create
                FROM patient_summary
                WHERE dt_create >= @dt_create;";
                cmd.Parameters.AddWithValue("@dt_create", SqliteHelper.FromDateTime(from));
            }
            else
            {
                cmd.CommandText = @"
                SELECT patient_initials, id, last_name, first_name, patronymic, name_suffix, policy_number, dt_create
                FROM patient_summary
                WHERE dt_create BETWEEN @from AND @to;";
                cmd.Parameters.AddWithValue("@from", SqliteHelper.FromDateTime(from));
                cmd.Parameters.AddWithValue("@to", SqliteHelper.FromDateTime(to.Value));
            }

            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    PatientInitials = reader.GetString(0),
                    Id = reader.GetInt32(1),
                    LastName = reader.GetString(2),
                    FirstName = reader.GetString(3),
                    Patronymic = SqliteHelper.GetStringNull(reader, 4),
                    NameSuffix = SqliteHelper.GetStringNull(reader, 5),
                    PolicyNumber = reader.GetString(6),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 7)
                });
            }
            return items;
        }
        public async Task<ObservableCollection<PatientSummaryModel>> FindByInitialsOrPolicyNumberAsync(SqliteConnection connection, string searchStr)
        {
            var cleanStr= searchStr
                .Replace("'", "''")
                .Replace("%", "\\%")
                .Replace("_", "\\_")
                .Replace("[", "\\[")
                .Replace("]", "\\]");

            ObservableCollection<PatientSummaryModel> items = [];

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT patient_initials, id, last_name, first_name, patronymic, name_suffix, policy_number, dt_create
                FROM patient_summary
                WHERE last_name LIKE @str 
                   OR first_name LIKE @str 
                   OR patronymic LIKE @str
                   OR name_suffix LIKE @str
                   OR policy_number LIKE @str
                ORDER BY id ASC LIMIT 100;";
            cmd.Parameters.AddWithValue("@str", $"%{cleanStr.Trim()}%");

            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    PatientInitials = reader.GetString(0),
                    Id = reader.GetInt32(1),
                    LastName = reader.GetString(2),
                    FirstName = reader.GetString(3),
                    Patronymic = SqliteHelper.GetStringNull(reader, 4),
                    NameSuffix = SqliteHelper.GetStringNull(reader, 5),
                    PolicyNumber = reader.GetString(6),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 7)
                });
            }
            return items;
        }
        public async Task<PatientModel?> FindByPatientIdAsync(SqliteConnection connection, int patientId)
        {
            PatientModel? item = null;
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    id, last_name, first_name, patronymic, name_suffix, birthday,
                    registration_address, dt_create, policy_number, working_position, job_category_id
                FROM patients
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", patientId);
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                item = new()
                {
                    Id = reader.GetInt32(0),
                    LastName = reader.GetString(1),
                    FirstName = reader.GetString(2),
                    Patronymic = SqliteHelper.GetStringNull(reader, 3),
                    NameSuffix = SqliteHelper.GetStringNull(reader, 4),
                    Birthday = SqliteHelper.GetDate(reader, 5),
                    RegistrationAddress = reader.GetString(6),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 7),
                    PolicyNumber = reader.GetString(8),
                    WorkingPosition = reader.GetString(9),
                    JobCategoryId = reader.GetInt32(10)
                };
            }
            return item;
        }
        public async Task<int> UpdateAsync(SqliteConnection connection, PatientModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE patients
                SET 
                    last_name=@last_name, first_name=@first_name, patronymic=@patronymic,
                    name_suffix=@name_suffix, birthday=@birthday, registration_address=@registration_address, 
                    dt_create=@dt_create, policy_number=@policy_number, working_position=@working_position,
                    job_category_id=@job_category_id
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@last_name", model.LastName);
            cmd.Parameters.AddWithValue("@first_name", model.FirstName);
            cmd.Parameters.AddWithValue("@patronymic", SqliteHelper.FromStringNull(model.Patronymic));
            cmd.Parameters.AddWithValue("@name_suffix", SqliteHelper.FromStringNull(model.NameSuffix));
            cmd.Parameters.AddWithValue("@birthday", SqliteHelper.FromDate(model.Birthday));
            cmd.Parameters.AddWithValue("@registration_address", model.RegistrationAddress);
            cmd.Parameters.AddWithValue("@dt_create", SqliteHelper.FromDateTime(model.DateTimeCreate));
            cmd.Parameters.AddWithValue("@policy_number", model.PolicyNumber);
            cmd.Parameters.AddWithValue("@working_position", model.WorkingPosition);
            cmd.Parameters.AddWithValue("@job_category_id", model.JobCategoryId);
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        public async Task<int> DeleteAsync(SqliteConnection connection, PatientModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM patients WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
