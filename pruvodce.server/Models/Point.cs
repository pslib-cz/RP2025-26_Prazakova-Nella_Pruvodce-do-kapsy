using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Point
    {
        [Key]
        public string PointId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Název je povinný")]
        public required string Label { get; set; }
        public string? Description { get; set; }

        public List<PointSubject> PointSubjects { get; set; } = new List<PointSubject>();
        public List<Teacher> Teachers { get; set; } = new();

        [Required(ErrorMessage = "Vyberte typ stanoviště")]
        public PointIcon? Icon { get; set; } = PointIcon.Jine;

        [Required(ErrorMessage = "Vyberte místnost")]
        public string? RoomId { get; set; }

        [Required(ErrorMessage = "Vyberte na jaké akci se koná")]
        public int? EventId { get; set; }
        public Event? Event { get; set; }

        public string? SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }

        public string? NoteId { get; set; }
        public StudentNote? Note { get; set; }
    }
}