using pruvodce.server.Models;
using System.Text.Json;

namespace pruvodce.server.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Buildings.Any())
                return;

            var buildingM = new Building
            {
                BuildingId = 1,
                Name = "Masarykova",
                Address = "Masarykova 460/3"
            };

            var floorM1 = new Floor
            {
                FloorId = 1,
                Name = "1. Patro",
                FloorNumber = 1,
                MapImageUrl = "/Mfirst.svg",
                Building = buildingM
            };

            var floorM2 = new Floor
            {
                FloorId = 2,
                Name = "2. Patro",
                FloorNumber = 2,
                MapImageUrl = "/Msecond.svg",
                Building = buildingM
            };

            var floorM3 = new Floor
            {
                FloorId = 3,
                Name = "3. Patro",
                FloorNumber = 3,
                MapImageUrl = "/Mthird.svg",
                Building = buildingM
            };

            floorM1.Rooms = LoadRoomsFromJson("Data/rooms/m-floor1.json");

            buildingM.Floors = new List<Floor> { floorM1, floorM2, floorM3 };

            var buildingT = new Building
            {
                BuildingId = 2,
                Name = "Tyršova",
                Address = "Tyršova 82/1"
            };

            var floorT1 = new Floor
            {
                FloorId = 4,
                Name = "1. Patro",
                FloorNumber = 1,
                MapImageUrl = "/Tfirst.svg",
                Building = buildingT
            };

            var floorT2 = new Floor
            {
                FloorId = 5,
                Name = "2. Patro",
                FloorNumber = 2,
                MapImageUrl = "/Tsecond.svg",
                Building = buildingT
            };

            var floorT3 = new Floor
            {
                FloorId = 6,
                Name = "3. Patro",
                FloorNumber = 3,
                MapImageUrl = "/Tthird.svg",
                Building = buildingT
            };

            floorT1.Rooms = LoadRoomsFromJson("Data/rooms/m-floor1.json");

            buildingT.Floors = new List<Floor> { floorT1, floorT2, floorT3 };


            context.Buildings.AddRange(buildingM, buildingT);
            context.Teachers.AddRange(teachers);

            context.SaveChanges();
        }

        private static List<Room> LoadRoomsFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<Room>();

                var json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var roomsData = JsonSerializer.Deserialize<List<RoomDto>>(json, options);

                return roomsData?.Select(r => new Room
                {
                    RoomId = r.RoomId,
                    Label = r.Label,
                    SvgData = r.SvgData,
                    CoordinateX = r.CoordinateX,
                    CoordinateY = r.CoordinateY,
                    Type = (RoomType)r.Type,
                    Icon = r.Icon,
                    Note = r.Note,
                    FloorId = r.FloorId
                }).ToList() ?? new List<Room>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba pøi naèítání {filePath}: {ex.Message}");
                return new List<Room>();
            }
        }

        private static List<Teacher> LoadTeachersFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<Teacher>();

                var json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var teachersData = JsonSerializer.Deserialize<List<TeacherDto>>(json, options);

                return teachersData?.Select(t => new Teacher
                {
                    TeacherId = t.TeacherId,
                    FirstN = t.FirstN,
                    LastN = t.LastN,
                    Degree = t.Degree,
                    Note = t.Note
                }).ToList() ?? new List<Teacher>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba pøi naèítání {filePath}: {ex.Message}");
                return new List<Teacher>();
            }
        }
    }

    public class RoomDto
    {
        public string RoomId { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string SvgData { get; set; } = string.Empty;
        public double? CoordinateX { get; set; }
        public double? CoordinateY { get; set; }
        public int Type { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public int FloorId { get; set; }
    }

    public class TeacherDto
    {
        public string TeacherId { get; set; } = string.Empty;
        public string FirstN { get; set; } = string.Empty;
        public string LastN { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}