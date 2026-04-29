using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public required string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public string? Description { get; set; }

        public List<Point> Points { get; set; } = new();

        [Required]
        public int? BuildingId { get; set; }
    }
}