using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Event : IValidatableObject
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Název je povinný")]
        [StringLength(150, ErrorMessage = "Název může mít maximálně 150 znaků")]
        public required string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public string? Description { get; set; }

        public List<Point> Points { get; set; } = new();

        public List<EventBuilding> EventBuildings { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "Konec akce musí být později než začátek.",
                    new[] { nameof(EndDate) }
                );
            }
        }
    }
}