using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class AdminUser
    {
        [Key]
        public int AdminUserId { get; set; }

        [Required(ErrorMessage = "Email je povinný")]
        [EmailAddress(ErrorMessage = "Zadejte platný email")]
        [RegularExpression(@"^[^@\s]+@pslib\.cz$", ErrorMessage = "Email musí být ze školní domény @pslib.cz")] //??
        public string Email { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;

    }
}