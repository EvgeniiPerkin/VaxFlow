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
                    (full_name, birthday, registration_address, 
                        dt_create, policy_number, working_position, job_category_id)
                VALUES 
                    (@full_name, @birthday, @registration_address,
                        @dt_create, @policy_number, @working_position, @job_category_id);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@full_name", model.FullName);
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
        public async Task<ObservableCollection<PatientModel>> FilteredByDateCreateAsync(SqliteConnection connection, DateTime from, DateTime? to = null)
        {
            ObservableCollection<PatientModel> items = [];

            using var cmd = connection.CreateCommand();
            if (to == null)
            {
                cmd.CommandText = @"
                SELECT
                    id, full_name, birthday, registration_address, dt_create, policy_number, working_position, job_category_id
                FROM patients
                WHERE dt_create >= @dt_create;";
                cmd.Parameters.AddWithValue("@dt_create", SqliteHelper.FromDateTime(from));
            }
            else
            {
                cmd.CommandText = @"
                SELECT 
                    id, full_name, birthday, registration_address, dt_create, policy_number, working_position, job_category_id
                FROM patients
                WHERE dt_create BETWEEN @from AND @to;";
                cmd.Parameters.AddWithValue("@from", SqliteHelper.FromDateTime(from));
                cmd.Parameters.AddWithValue("@to", SqliteHelper.FromDateTime(to.Value));
            }

            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Birthday = SqliteHelper.GetDate(reader, 2),
                    RegistrationAddress = reader.GetString(3),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 4),
                    PolicyNumber = reader.GetString(5),
                    WorkingPosition = reader.GetString(6),
                    JobCategoryId = reader.GetInt32(7)
                });
            }
            return items;
        }
        public async Task<ObservableCollection<PatientModel>> FindByInitialsOrPolicyNumberAsync(SqliteConnection connection, string searchStr)
        {
            var cleanStr= searchStr
                .Replace("'", "''")
                .Replace("%", "\\%")
                .Replace("_", "\\_")
                .Replace("[", "\\[")
                .Replace("]", "\\]");

            ObservableCollection<PatientModel> items = [];

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT
                    id, full_name, birthday, registration_address, dt_create, policy_number, working_position, job_category_id
                FROM patients
                WHERE full_name LIKE @str
                   OR policy_number LIKE @str
                ORDER BY id ASC LIMIT 100;";
            cmd.Parameters.AddWithValue("@str", $"%{cleanStr.Trim()}%");

            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Birthday = SqliteHelper.GetDate(reader, 2),
                    RegistrationAddress = reader.GetString(3),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 4),
                    PolicyNumber = reader.GetString(5),
                    WorkingPosition = reader.GetString(6),
                    JobCategoryId = reader.GetInt32(7)
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
                    id, full_name, birthday,
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
                    FullName = reader.GetString(1),
                    Birthday = SqliteHelper.GetDate(reader, 2),
                    RegistrationAddress = reader.GetString(3),
                    DateTimeCreate = SqliteHelper.GetDateTime(reader, 4),
                    PolicyNumber = reader.GetString(5),
                    WorkingPosition = reader.GetString(6),
                    JobCategoryId = reader.GetInt32(7)
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
                    full_name=@full_name, birthday=@birthday, registration_address=@registration_address, 
                    policy_number=@policy_number, working_position=@working_position,
                    job_category_id=@job_category_id
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@full_name", model.FullName);
            cmd.Parameters.AddWithValue("@birthday", SqliteHelper.FromDate(model.Birthday));
            cmd.Parameters.AddWithValue("@registration_address", model.RegistrationAddress);
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
