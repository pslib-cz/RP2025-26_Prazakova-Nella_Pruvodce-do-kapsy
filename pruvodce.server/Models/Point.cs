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

        public List<PointSubject> PointSubjects { get; set; } = new();

        public List<PointTeacher> PointTeachers { get; set; } = new();

        [Required(ErrorMessage = "Vyberte typ stanoviště")]
        public PointIcon Icon { get; set; } = PointIcon.Jine;

        [Required(ErrorMessage = "Vyberte místnost")]
        public string RoomId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vyberte zaměření")]
        public required string SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }

        public bool AreStudents { get; set; } = false;

        public List<EventPoint> EventPoints { get; set; } = new();
    }
}