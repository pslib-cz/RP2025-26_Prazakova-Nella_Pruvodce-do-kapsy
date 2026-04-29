using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Point
    {
        [Key]
        public string PointId { get; set; } = string.Empty;

        public required string Label { get; set; }
        public string? Description { get; set; }

        public List<Subject> Subjects { get; set; } = new();
        public List<Teacher> Teachers { get; set; } = new();

        public string? Note { get; set; }
        public string? Icon { get; set; }

        [Required]
        public string? RoomId { get; set; }

        public int? EventId { get; set; }
        public Event? Event { get; set; }

        public string? SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
    }
}