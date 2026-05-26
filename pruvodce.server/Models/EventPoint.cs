using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class EventPoint
    {
        public int EventId { get; set; }
        public Event? Event { get; set; }

        public string PointId { get; set; } = string.Empty;
        public Point? Point { get; set; }
    }
}