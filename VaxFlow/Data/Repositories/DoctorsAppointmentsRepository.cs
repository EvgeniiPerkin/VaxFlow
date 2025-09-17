using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class DoctorsAppointmentsRepository
    {
        public async Task<int> CreateAsync(SqliteConnection connection, DoctorsAppointmentsModel da)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO doctors_appointments (doctor_id, patient_id, party_id, dt_of_appointment)
                VALUES (@doctor_id, @patient_id, @party_id, @dt_of_appointment);";
            cmd.Parameters.AddWithValue("@doctor_id", da.DoctorId);
            cmd.Parameters.AddWithValue("@patient_id", da.PatientId);
            cmd.Parameters.AddWithValue("@party_id", da.PartyId);
            cmd.Parameters.AddWithValue("@dt_of_appointment", SqliteHelper.FromDateTime(da.DateTimeOfAppointment));
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        public async Task<int> DeleteAsync(SqliteConnection connection, DoctorsAppointmentsModel da)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM doctors_appointments WHERE
                doctor_id=@doctor_id AND patient_id=@patient_id AND party_id=@party_id;";
            cmd.Parameters.AddWithValue("@doctor_id", da.DoctorId);
            cmd.Parameters.AddWithValue("@patient_id", da.PatientId);
            cmd.Parameters.AddWithValue("@party_id", da.PartyId);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
