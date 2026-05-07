using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;
using pruvodce.server.Services;

namespace pruvodce.server.Pages.Points
{
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
        public int? EventId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 6;

        public SelectList EventItems { get; set; } = default!;

        public PagedResult<PointRowViewModel> Points { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (PageNumber < 1)
            {
                PageNumber = 1;
            }

            if (PageSize < 1)
            {
                PageSize = 6;
            }

            await LoadEventItemsAsync();
            await LoadPointsAsync();
        }

        private async Task LoadEventItemsAsync()
        {
            var events = await _context.Events
                .AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            EventItems = new SelectList(
                events,
                "EventId",
                "Name",
                EventId
            );
        }

        private async Task LoadPointsAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();

            var roomLookup = mapData.Buildings
                .SelectMany(building => building.Floors.SelectMany(floor => floor.Rooms.Select(room => new
                {
                    room.RoomId,
                    BuildingName = building.Name,
                    RoomLabel = string.IsNullOrWhiteSpace(room.Label) ? room.RoomId : room.Label
                })))
                .GroupBy(room => room.RoomId)
                .ToDictionary(
                    group => group.Key,
                    group => group.First()
                );

            var query = _context.Points
                .Include(p => p.Event)
                .Include(p => p.Specialization)
                .Include(p => p.Note)
                .AsNoTracking()
                .AsQueryable();

            if (EventId.HasValue)
            {
                query = query.Where(p => p.EventId == EventId.Value);
            }

            var pointsFromDb = await query
                .OrderBy(p => p.Label)
                .ToListAsync();

            var rows = pointsFromDb
                .Select(point =>
                {
                    roomLookup.TryGetValue(point.RoomId ?? string.Empty, out var roomInfo);

                    var icon = point.Icon ?? PointIcon.Jine;

                    return new PointRowViewModel
                    {
                        PointId = point.PointId,
                        Icon = icon,
                        SpecializationType = point.Specialization?.Type,
                        Label = point.Label,
                        Room = roomInfo?.RoomLabel ?? point.RoomId ?? "-",
                        Building = roomInfo?.BuildingName ?? "-",
                        EventName = point.Event?.Name ?? "-",
                        SpecializationName = point.Specialization?.Name ?? "-",
                        SearchText = string.Join(" ", new[]
                        {
                            point.Label,
                            point.RoomId,
                            icon.ToString(),
                            roomInfo?.RoomLabel,
                            roomInfo?.BuildingName,
                            point.Event?.Name,
                            point.Specialization?.Name,
                            point.Description,
                            point.Note == null ? "" : point.Note.Text
                        }.Where(value => !string.IsNullOrWhiteSpace(value)))
                    };
                })
                .ToList();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var search = Search.Trim();

                rows = rows
                    .Where(row => row.SearchText.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            rows = rows
                .OrderBy(row => row.EventName)
                .ThenBy(row => row.Building)
                .ThenBy(row => row.Room)
                .ThenBy(row => row.Label)
                .ToList();

            var totalItems = rows.Count;

            var items = rows
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

        public class PointRowViewModel
        {
            public string PointId { get; set; } = string.Empty;
            public PointIcon Icon { get; set; } = PointIcon.Jine;
            public FieldType? SpecializationType { get; set; }

            public string Label { get; set; } = string.Empty;
            public string Room { get; set; } = "-";
            public string Building { get; set; } = "-";
            public string EventName { get; set; } = "-";
            public string SpecializationName { get; set; } = "-";
            public string SearchText { get; set; } = string.Empty;

            public string IconPath => $"/icons/{Icon.ToString().ToLowerInvariant()}.svg";
            public string IconText => Icon.ToString();

            public string FieldTypeClass => SpecializationType?.ToString().ToLowerInvariant() ?? "default";
        }
    }
}