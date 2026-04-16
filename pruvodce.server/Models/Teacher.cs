using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Teacher
    {
        [Key]
        public string TeacherId { get; set; } = string.Empty;
        public string? Degree { get; set; }
        public required string FirstN { get; set; }
        public required string LastN { get; set; }

        public string? Note { get; set; } // poznámka od studentů

        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }
}