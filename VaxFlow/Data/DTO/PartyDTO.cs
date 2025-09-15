using System;

namespace VaxFlow.Data.DTO
{
    public class PartyDTO
    {
        public int Id { get; set; }
        public int VaccineId { get; set; }
        public string PartyName { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime DateTimeCreate { get; set; } = DateTime.Now;
    }
}
