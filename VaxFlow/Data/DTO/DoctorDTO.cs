namespace VaxFlow.Data.DTO
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? Patronymic { get; set; }
        public string? NameSuffix { get; set; }
        public bool IsDismissed { get; set; } = false;
    }
}
