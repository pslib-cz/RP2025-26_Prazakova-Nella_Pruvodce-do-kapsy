using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class StudentNote
    {
        [Key]
        public string StudentNoteId { get; set; } = Guid.NewGuid().ToString();

        public string? Text { get; set; }

        public string? StudentName { get; set; } = "Student";

        public FieldType? StudentField { get; set; }

        public int? StudentYear { get; set; }
    }
}