using System;

namespace VaxFlow.Data.DTO
{
    public class DoctorsAppointmentsDTO
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int VaccineId { get; set; }
        public DateTime DateTimeOfAppointment { get; set; } = DateTime.Now;
    }
}
