using System.Text.Json;
using pruvodce.server.Models.MapData;

namespace pruvodce.server.Services
{
    public class MapDataService
    {
        private readonly IWebHostEnvironment _environment;

        public MapDataService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<MapDataRoot> GetMapDataAsync()
        {
            var mapPath = Path.Combine(_environment.WebRootPath, "data", "map.json");

            if (!File.Exists(mapPath))
            {
                return new MapDataRoot();
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var mapJson = await File.ReadAllTextAsync(mapPath);

            var mapData = JsonSerializer.Deserialize<MapDataRoot>(mapJson, jsonOptions)
                          ?? new MapDataRoot();

            foreach (var building in mapData.Buildings)
            {
                foreach (var floor in building.Floors)
                {
                    if (string.IsNullOrWhiteSpace(floor.RoomsUrl))
                    {
                        continue;
                    }

                    var relativeRoomsPath = floor.RoomsUrl
                        .TrimStart('/')
                        .Replace('/', Path.DirectorySeparatorChar);

                    var roomsPath = Path.Combine(_environment.WebRootPath, relativeRoomsPath);

                    if (!File.Exists(roomsPath))
                    {
                        floor.Rooms = new List<MapRoom>();
                        continue;
                    }

                    var roomsJson = await File.ReadAllTextAsync(roomsPath);

                    floor.Rooms = JsonSerializer.Deserialize<List<MapRoom>>(roomsJson, jsonOptions)
                                  ?? new List<MapRoom>();
                }
            }

            return mapData;
        }
    }
}