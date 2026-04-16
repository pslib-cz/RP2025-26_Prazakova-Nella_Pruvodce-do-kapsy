using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Subject
    {
        [Key]
        public string SubjectId { get; set; } = string.Empty;
        public required string Name { get; set; }
        public required string Acronym { get; set; }

        public string? Note { get; set; } //poznámka od studentů

        public List<Specialization> Specialization { get; set; } = new List<Specialization>();

        public string? RoomId { get; set; }
        public Room? Room { get; set; }
    }
}