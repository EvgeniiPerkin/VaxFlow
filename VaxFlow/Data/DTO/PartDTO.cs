using System.ComponentModel.DataAnnotations;

namespace VaxFlow.Data.DTO
{
    /// <summary>
    /// Часть документа(шаблона)
    /// </summary>
    public class PartDTO
    {
        public int Id { get; set; }
        public int PatternId { get; set; }
        public int SerialNumber { get; set; }
        public string Body { get; set; } = string.Empty;
        public string? Desc { get; set; }
        public bool IsUrl { get; set; } = false;
        public string? Url { get; set; }
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public bool IsUnderline { get; set; } = false;
    }
}
