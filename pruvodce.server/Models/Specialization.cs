using System;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Specialization
    {
        [Key]
        public string SpecializationId { get; set; } = string.Empty;
        public required string Name { get; set; }
        public required string Description { get; set; } //kde se žáci využijou, co se uèí

        public FieldType Type { get; set; }
    }
}