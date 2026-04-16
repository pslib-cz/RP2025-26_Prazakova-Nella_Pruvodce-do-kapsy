using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Point
    {
        [Key]
        public string PointId { get; set; } = string.Empty;
        public required string Label { get; set; }
        public string? Description { get; set; }

        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public double LabelX { get; set; }
        public double LabelY { get; set; }

        public string? Note { get; set; } // poznámka od studentů
        public string? Icon { get; set; } // název ikonky, svg v css (možná enum?)

        public string? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public required string RoomId { get; set; }
        public Room? Room { get; set; }

        public int? EventId { get; set; }
        public Event? Event { get; set; }
    }
}