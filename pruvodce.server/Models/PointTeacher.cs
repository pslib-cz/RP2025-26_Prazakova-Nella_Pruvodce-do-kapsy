using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class PointTeacher
    {
        [Key]
        public string PointTeacherId { get; set; } = Guid.NewGuid().ToString();

        public string PointId { get; set; } = string.Empty;
        public Point Point { get; set; } = null!;

        public string TeacherId { get; set; } = string.Empty;
        public Teacher Teacher { get; set; } = null!;
    }
}