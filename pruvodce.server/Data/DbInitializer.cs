using Microsoft.EntityFrameworkCore;
using pruvodce.server.Models;

namespace pruvodce.server.Data
{
    public static class DbInitializer
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            //  Budova
            modelBuilder.Entity<Building>().HasData(
                new Building
                {
                    BuildingId = 1,
                    Name = "Hlavní budova",
                    Address = "Školní 1"
                }
            );

            // Patro
            modelBuilder.Entity<Floor>().HasData(
                new Floor
                {
                    FloorId = 1,
                    Name = "Pøízemí",
                    BuildingId = 1,
                    SvgOutline = "M 0 0 L 100 100"
                }
            );

            //  Místnost
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    RoomId = "A214",
                    Label = "Laboratoø IT",
                    FloorId = 1,
                    Type = RoomType.Specialized,
                    SvgData = "..."
                }
            );

            // Event
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    EventId = 1,
                    Name = "DOD 2026 Leden",
                    IsActive = true,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1)
                }
            );

            // Stanovištì
            modelBuilder.Entity<Point>().HasData(
                new Point
                {
                    PointId = "P1",
                    Label = "Stanovištì robotiky",
                    RoomId = "A214",
                    EventId = 1,
                    LabelX = 50, 
                   LabelY = 50
                }
            );
        }
    }
}