using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace pruvodce.server.Models
{

    public class Event : IValidatableObject
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Název je povinný")]
        [StringLength(150, ErrorMessage = "Název může mít maximálně 150 znaků")]
        public required string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = false;

        public string? Description { get; set; }

        public List<EventPoint> EventPoints { get; set; } = new();

        [NotMapped] //at muzu psat event.Points 
        public List<Point> Points => EventPoints.Select(ep => ep.Point!).ToList();
        public List<EventBuilding> EventBuildings { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue && EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "Konec akce musí být později než začátek.",
                    new[] { nameof(EndDate) }
                );
            }
        }

        public bool IsCurrentlyActive()
        {
            if (!StartDate.HasValue && !EndDate.HasValue)
            {
                return IsActive;
            }
            var now = DateTime.Now;
            var withinDateRange = (!StartDate.HasValue || now >= StartDate)
                            && (!EndDate.HasValue || now <= EndDate);
            return withinDateRange && IsActive;
        }
    }
}