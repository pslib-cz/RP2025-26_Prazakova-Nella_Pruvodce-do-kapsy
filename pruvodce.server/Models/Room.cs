using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Room
    {
        [Key]
        public string RoomId { get; set; } = string.Empty;
        public required string SvgData { get; set; }
        public required string Label { get; set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public string? Icon { get; set; }
        public double? CoordinateX { get; set; }
        public double? CoordinateY { get; set; }

        public required RoomType Type { get; set; }

        public string? Note { get; set; } //poznámka od studentů

        public required int FloorId { get; set; }
        public Floor? Floor { get; set; }

        public List<Point> Points { get; set; } = new List<Point>();
    }
}