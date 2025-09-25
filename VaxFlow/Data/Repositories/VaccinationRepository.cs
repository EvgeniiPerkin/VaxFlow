using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class VaccinationRepository
    {
        public async Task<ObservableCollection<VaccinationSummaryModel>> FindByPatientId(SqliteConnection connection, int patientId)
        {
            ObservableCollection<VaccinationSummaryModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT doctor_id, patient_id, party_id, dt_of_vaccination, doctor_initials, desc_party
                FROM vaccination_summary
                WHERE patient_id=@patient_id;";
            cmd.Parameters.AddWithValue("@patient_id", patientId);
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    DoctorId = reader.GetInt32(0),
                    PatientId = reader.GetInt32(1),
                    PartyId = reader.GetInt32(2),
                    DateTimeOfVaccination = SqliteHelper.GetDateTime(reader, 3),
                    DoctorInitials = reader.GetString(4),
                    DescParty = reader.GetString(5)
                });
            }
            return items;
        }
        public async Task<int> CreateAsync(SqliteConnection connection, VaccinationModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO vaccinations (doctor_id, patient_id, party_id, dt_of_vaccination)
                VALUES (@doctor_id, @patient_id, @party_id, @dt_of_vaccination);";
            cmd.Parameters.AddWithValue("@doctor_id", model.DoctorId);
            cmd.Parameters.AddWithValue("@patient_id", model.PatientId);
            cmd.Parameters.AddWithValue("@party_id", model.PartyId);
            cmd.Parameters.AddWithValue("@dt_of_vaccination", SqliteHelper.FromDateTime(model.DateTimeOfVaccination));
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        public async Task<int> DeleteAsync(SqliteConnection connection, VaccinationModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM vaccinations WHERE
                doctor_id=@doctor_id AND patient_id=@patient_id AND party_id=@party_id;";
            cmd.Parameters.AddWithValue("@doctor_id", model.DoctorId);
            cmd.Parameters.AddWithValue("@patient_id", model.PatientId);
            cmd.Parameters.AddWithValue("@party_id", model.PartyId);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
