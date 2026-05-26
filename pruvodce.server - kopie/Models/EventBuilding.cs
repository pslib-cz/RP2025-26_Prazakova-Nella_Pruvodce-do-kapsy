namespace pruvodce.server.Models
{
    public class EventBuilding
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = default!;

        public int BuildingId { get; set; }
    }
}