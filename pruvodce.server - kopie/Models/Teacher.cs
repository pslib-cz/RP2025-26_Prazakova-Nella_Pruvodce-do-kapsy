using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 
namespace pruvodce.server.Models
{
    public class Teacher
    {
        [Key]
        public string TeacherId { get; set; } = string.Empty;
 
        [StringLength(20, ErrorMessage = "Titul může mít maximálně 20 znaků")]
        public string? Degree { get; set; }
 
        [Required(ErrorMessage = "Jméno je povinné")]
        [StringLength(40, ErrorMessage = "Jméno může mít maximálně 40 znaků")]
        public required string FirstN { get; set; }
 
        [Required(ErrorMessage = "Příjmení je povinné")]
        [StringLength(40, ErrorMessage = "Příjmení může mít maximálně 40 znaků")]
        public required string LastN { get; set; }
 
        public List<StudentNote>? Notes { get; set; } = new();
 
        public string? SelectedNoteIds { get; set; }
 
        public List<PointTeacher> PointTeachers { get; set; } = new();
    }
}