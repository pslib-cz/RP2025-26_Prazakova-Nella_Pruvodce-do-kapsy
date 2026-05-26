using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Specialization
    {
        [Key]
        public string SpecializationId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Název je povinný")]
        [StringLength(150, ErrorMessage = "Název může mít maximálně 150 znaků")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Popis je povinný")]
        [StringLength(1000, ErrorMessage = "Popis může mít maximálně 1000 znaků")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Vyberte typ oboru")]
        public FieldType? Type { get; set; }

        public List<Point> Points { get; set; } = new List<Point>();
    }
}