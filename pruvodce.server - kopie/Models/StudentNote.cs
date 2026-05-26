using System.ComponentModel.DataAnnotations;
 
namespace pruvodce.server.Models
{
    public class StudentNote
    {
        [Key]
        public string StudentNoteId { get; set; } = Guid.NewGuid().ToString();
 
        [Required]
        [MaxLength(300)]
        public string Text { get; set; } = string.Empty;
 
        [Required]
        [MaxLength(50)]
        public string StudentName { get; set; } = string.Empty;
 
        [Required]
        [MaxLength(3)]
        public string StudentClass { get; set; } = string.Empty;
 
        [Required]
        public string TargetType { get; set; } = string.Empty.ToUpper();
 
        public string? TeacherId { get; set; }
        public string? SubjectId { get; set; }
        public Teacher? Teacher { get; set; }
        public Subject? Subject { get; set; }
 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}