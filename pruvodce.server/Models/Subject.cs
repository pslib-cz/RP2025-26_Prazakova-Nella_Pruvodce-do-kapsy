using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Subject
    {
        [Key]
        public string SubjectId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Název je povinný")]
        [StringLength(50, ErrorMessage = "Název může mít maximálně 550 znaků")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Zkratka je povinná")]
        [StringLength(7, ErrorMessage = "Zkratka může mít maximálně 7 znaků")]
        public required string Acronym { get; set; }

        public string? NoteId { get; set; }
        public StudentNote? Note { get; set; }

        public List<PointSubject> PointSubjects { get; set; } = new();
    }
}