using System;

namespace VaxFlow.Data.DTO
{
    public class DoctorsAppointmentsDTO
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int PartyId { get; set; }
        public DateTime DateTimeOfAppointment { get; set; } = DateTime.Now;
    }
}
