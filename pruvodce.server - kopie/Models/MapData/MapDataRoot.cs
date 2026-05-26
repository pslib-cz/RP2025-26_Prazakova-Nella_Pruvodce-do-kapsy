namespace pruvodce.server.Models.MapData
{
    public class MapDataRoot
    {
        public List<MapBuilding> Buildings { get; set; } = new();
    }

    public class MapBuilding
    {
        public int BuildingId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public List<MapFloor> Floors { get; set; } = new();
    }

    public class MapFloor
    {
        public int FloorId { get; set; }
        public int? FloorNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? BackgroundUrl { get; set; }
        public string? DetailUrl { get; set; }
        public string? RoomsUrl { get; set; }
        public List<MapRoom> Rooms { get; set; } = new();
    }

    public class MapRoom
    {
        public string RoomId { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;

        public string? SvgOutline { get; set; }
        public string? SvgData { get; set; }
        public string? ClipPathId { get; set; }

        public string? InteriorImageUrl { get; set; }
        public double? InteriorX { get; set; }
        public double? InteriorY { get; set; }
        public double? InteriorWidth { get; set; }
        public double? InteriorHeight { get; set; }

        public double? CoordinateX { get; set; }
        public double? CoordinateY { get; set; }

        public int? Type { get; set; }
        public int FloorId { get; set; }

        public string? Icon { get; set; }
    }
}