using System;
using System.Collections.Generic;
using VaxFlow.Models;

namespace VaxFlow.Data.DTO
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? Patronymic { get; set; }
        public string? NameSuffix { get; set; }
        public DateTime Birthday { get; set; } = DateTime.MinValue;
        public string RegistrationAddress { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string WorkingPosition { get; set; } = string.Empty;
        public int JobCategoryId { get; set; }
        public DateTime DateTimeCreate { get; set; } = DateTime.Now;
        public List<DoctorsAppointmentsModel> Appointments { get; set; } = [];
    }
}
