using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public required string Name { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } //jestli je zrovna vidět na webu

        public string? Description { get; set; }

        public List<Point> Points { get; set; } = new List<Point>();
    }
}