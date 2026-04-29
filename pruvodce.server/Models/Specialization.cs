using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Specialization
    {
        [Key]
        public string SpecializationId { get; set; } = string.Empty;

        public required string Name { get; set; }
        public required string Description { get; set; }

        [Required]
        public FieldType? Type { get; set; }
        [Required]
        public SpecializationIcon? Icon { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();
    }
}