using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using pruvodce.server.Data;
using pruvodce.server.Models;
using pruvodce.server.Services;

namespace pruvodce.server.Pages
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly MapDataService _mapDataService;

        public IndexModel(ApplicationDbContext context, MapDataService mapDataService)
        {
            _context = context;
            _mapDataService = mapDataService;
        }

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? BuildingId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public PagedResult<PointRowViewModel> Items { get; set; } = new();

        public List<SelectListItem> BuildingItems { get; set; } = new();

        public PagedResult<PointRowViewModel> Points { get; set; } = new();

        public string CurrentEventTitle { get; set; } = "žádný aktivní event";

        public int TotalTeachersCount { get; set; }
        public int TotalSpecializationsCount { get; set; }
        public int TotalSubjectsCount { get; set; }

        public async Task OnGetAsync()
        {
            if (PageNumber < 1)
            {
                PageNumber = 1;
            }

            if (PageSize < 1)
            {
                PageSize = 10;
            }

            await LoadBuildingItemsAsync();
            await LoadCountsAsync();
            await LoadPointsAsync();
        }


        private async Task LoadBuildingItemsAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();

            BuildingItems = mapData.Buildings
                .OrderBy(b => b.Name)
                .Select(b => new SelectListItem
                {
                    Value = b.BuildingId.ToString(),
                    Text = b.Name
                })
                .ToList();
        }

        private async Task LoadCountsAsync()
        {
            TotalTeachersCount = await _context.Teachers.CountAsync();
            TotalSpecializationsCount = await _context.Specializations.CountAsync();
            TotalSubjectsCount = await _context.Subjects.CountAsync();
        }

        private async Task LoadPointsAsync()
        {
            var visibleEvents = await GetCurrentlyVisibleEventsAsync();
            var roomBuildingMap = await GetRoomBuildingMapAsync();
            var buildingNameMap = await GetBuildingNameMapAsync();

            if (BuildingId.HasValue)
            {
                visibleEvents = visibleEvents
                    .Where(e => e.BuildingId == BuildingId.Value)
                    .ToList();
            }

            if (visibleEvents.Count == 0)
            {
                Points = new PagedResult<PointRowViewModel>
                {
                    Items = new List<PointRowViewModel>(),
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalItems = 0
                };
                CurrentEventTitle = "žádný aktivní event";
                return;
            }

            CurrentEventTitle = string.Join(", ",
                visibleEvents
                    .Select(e => e.Event.Name)
                    .Distinct()
                    .ToList());

            var visiblePairs = visibleEvents
                .Select(e => new { e.EventId, e.BuildingId })
                .ToList();

            var visibleEventIds = visiblePairs
                .Select(e => e.EventId)
                .Distinct()
                .ToList();

            var rawPoints = await _context.EventPoints
                .Where(ep => visibleEventIds.Contains(ep.EventId))
                .Include(ep => ep.Point)
                    .ThenInclude(p => p!.Specialization)
                .AsNoTracking()
                .ToListAsync();

            var filtered = rawPoints
                .Where(ep =>
                    ep.Point != null &&
                    !string.IsNullOrWhiteSpace(ep.Point.RoomId) &&
                    roomBuildingMap.TryGetValue(ep.Point.RoomId, out var pointBuildingId) &&
                    visiblePairs.Any(v =>
                        v.EventId == ep.EventId &&
                        v.BuildingId == pointBuildingId))
                .Select(ep =>
                {
                    var pointBuildingId = roomBuildingMap[ep.Point!.RoomId!];

                    return new PointRowViewModel
                    {
                        Label = ep.Point.Label,
                        Building = buildingNameMap.TryGetValue(pointBuildingId, out var buildingName)
                            ? buildingName
                            : "-",
                        Room = ep.Point.RoomId ?? "-",
                        Specialization = ep.Point.Specialization?.Name ?? "-",
                        AreStudents = ep.Point.AreStudents
                    };
                })
                .DistinctBy(p => p.Label + p.Room)
                .ToList();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var search = Search.Trim().ToLower();

                filtered = filtered
                    .Where(p =>
                        p.Label.ToLower().Contains(search) ||
                        p.Building.ToLower().Contains(search) ||
                        p.Room.ToLower().Contains(search) ||
                        p.Specialization.ToLower().Contains(search))
                    .ToList();
            }

            var orderedPoints = filtered
                .OrderBy(p => p.Building)
                .ThenBy(p => p.Room)
                .ThenBy(p => p.Label)
                .ToList();

            var totalItems = orderedPoints.Count;

            var items = orderedPoints
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            Points = new PagedResult<PointRowViewModel>
            {
                Items = items,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalItems = totalItems
            };
        }

        private async Task<List<VisibleEventForBuilding>> GetCurrentlyVisibleEventsAsync()
        {
            var now = DateTime.Now;

            var activeEventsWithPoints = await _context.Events
                .Include(e => e.EventPoints)
                    .ThenInclude(ep => ep.Point)
                .Where(e => e.IsActive)
                .AsNoTracking()
                .ToListAsync();

            var roomBuildingMap = await GetRoomBuildingMapAsync();

            var visibleEvents = new List<VisibleEventForBuilding>();

            foreach (var evt in activeEventsWithPoints)
            {
                var withinDateRange = (!evt.StartDate.HasValue || evt.StartDate <= now)
                                    && (!evt.EndDate.HasValue || evt.EndDate >= now);

                if (!withinDateRange)
                    continue;

                foreach (var ep in evt.EventPoints)
                {
                    if (ep.Point?.RoomId != null &&
                        roomBuildingMap.TryGetValue(ep.Point.RoomId, out var buildingId))
                    {
                        visibleEvents.Add(new VisibleEventForBuilding
                        {
                            EventId = evt.EventId,
                            Event = evt,
                            BuildingId = buildingId
                        });
                    }
                }
            }

            return visibleEvents;
        }

        private async Task<Dictionary<string, int>> GetRoomBuildingMapAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();

            return mapData.Buildings
                .SelectMany(building =>
                    building.Floors.SelectMany(floor =>
                        floor.Rooms.Select(room => new
                        {
                            room.RoomId,
                            building.BuildingId
                        })))
                .Where(x => !string.IsNullOrWhiteSpace(x.RoomId))
                .GroupBy(x => x.RoomId)
                .ToDictionary(g => g.Key, g => g.First().BuildingId);
        }

        private async Task<Dictionary<int, string>> GetBuildingNameMapAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();

            return mapData.Buildings
                .ToDictionary(b => b.BuildingId, b => b.Name);
        }

        public class PointRowViewModel
        {
            public string Label { get; set; } = string.Empty;
            public string Building { get; set; } = "-";
            public string Room { get; set; } = "-";
            public string Specialization { get; set; } = "-";
            public bool AreStudents { get; set; }
        }

        private class VisibleEventForBuilding
        {
            public int EventId { get; set; }
            public int BuildingId { get; set; }
            public Event Event { get; set; } = default!;
        }
    }
}