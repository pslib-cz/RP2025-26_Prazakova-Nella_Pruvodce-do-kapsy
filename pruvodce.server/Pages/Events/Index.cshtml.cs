using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;
using pruvodce.server.Services;

namespace pruvodce.server.Pages.Events
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
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 3;

        // Pro otevření modalu s dostupnými stanovišti
        [BindProperty(SupportsGet = true)]
        public int? AddPointsEventId { get; set; }

        // Pro toggle aktivace - z route
        [BindProperty(SupportsGet = true)]
        public int? ToggleEventId { get; set; }

        // Pro toggle aktivace - hodnota z formuláře
        [BindProperty]
        public bool? IsActive { get; set; }

        // Pro přidání stanovišť
        [BindProperty]
        public int? AddPointsEventIdForm { get; set; }

        [BindProperty]
        public string[]? SelectedPointIds { get; set; }

        // Pro odebrání stanoviště
        [BindProperty]
        public int RemoveEventId { get; set; }

        [BindProperty]
        public string RemovePointId { get; set; } = string.Empty;

        public PagedResult<Event> Events { get; set; } = new();
        public Dictionary<int, List<PointDto>> EventPointsMap { get; set; } = new();
        public List<PointDto> AvailablePointsForModal { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (PageNumber < 1) PageNumber = 1;
            if (PageSize < 1) PageSize = 3;

            var mapData = await _mapDataService.GetMapDataAsync();
            var allBuildings = mapData.Buildings.ToDictionary(b => b.BuildingId);

            var query = _context.Events
                .Include(e => e.EventPoints)
                    .ThenInclude(ep => ep.Point)
                        .ThenInclude(p => p!.Specialization)
                .OrderByDescending(e => e.CreatedAt)
                .AsNoTracking();

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            foreach (var item in items)
            {
                var pointDtos = new List<PointDto>();
                foreach (var ep in item.EventPoints)
                {
                    Console.WriteLine($"[GET] Event {item.EventId} IsActive={item.IsActive}");

                    if (ep.Point != null)
                    {
                        pointDtos.Add(new PointDto
                        {
                            PointId = ep.PointId,
                            Label = ep.Point.Label,
                            Description = ep.Point.Description,
                            Icon = ep.Point.Icon.ToString(),
                            SpecializationName = ep.Point.Specialization?.Name ?? string.Empty
                        });
                    }
                }
                EventPointsMap[item.EventId] = pointDtos;
            }

            // Pokud je otevřen modal, načti dostupná stanoviště
            if (AddPointsEventId.HasValue)
            {
                var evt = await _context.Events
                    .Include(e => e.EventPoints)
                    .FirstOrDefaultAsync(e => e.EventId == AddPointsEventId);

                if (evt != null)
                {
                    var usedPointIds = evt.EventPoints.Select(ep => ep.PointId).ToHashSet();
                    var allPoints = await _context.Points
                        .Include(p => p.Specialization)
                        .Where(p => !usedPointIds.Contains(p.PointId))
                        .ToListAsync();

                    AvailablePointsForModal = allPoints.Select(p => new PointDto
                    {
                        PointId = p.PointId,
                        Label = p.Label,
                        Description = p.Description,
                        Icon = p.Icon.ToString(),
                        SpecializationName = p.Specialization?.Name ?? string.Empty
                    }).ToList();
                }
            }

            Events = new PagedResult<Event>
            {
                Items = items,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalItems = totalItems
            };
        }

        public async Task<IActionResult> OnPostToggleActiveAsync()
        {
            Console.WriteLine($"[TOGGLE] ToggleEventId={ToggleEventId}, IsActive={IsActive}");

            if (!ToggleEventId.HasValue)
            {
                Console.WriteLine("[TOGGLE] ToggleEventId je null -> redirect bez uložení");
                return RedirectToPage();
            }

            var evt = await _context.Events.FindAsync(ToggleEventId.Value);
            Console.WriteLine($"[TOGGLE] Event nalezen: {evt != null}, aktuální IsActive={evt?.IsActive}");

            if (evt == null)
                return NotFound();

            evt.IsActive = IsActive ?? false;
            Console.WriteLine($"[TOGGLE] Nastavuji IsActive={evt.IsActive}");
            
            _context.Events.Update(evt);
            var saved = await _context.SaveChangesAsync();
            Console.WriteLine($"[TOGGLE] SaveChanges uložil {saved} řádků");

            Response.Headers["Cache-Control"] = "no-cache, no-store";
            Response.Headers["Pragma"] = "no-cache";
            return RedirectToPage();
        }

        // Přidání stanovišť k akci
        public async Task<IActionResult> OnPostAddPointsAsync()
        {
            if (!AddPointsEventIdForm.HasValue || SelectedPointIds == null || !SelectedPointIds.Any())
                return RedirectToPage();

            var evt = await _context.Events
                .Include(e => e.EventPoints)
                .FirstOrDefaultAsync(e => e.EventId == AddPointsEventIdForm.Value);

            if (evt == null)
                return NotFound();

            var existingPoints = evt.EventPoints.Select(ep => ep.PointId).ToHashSet();

            foreach (var pointId in SelectedPointIds)
            {
                if (!existingPoints.Contains(pointId))
                {
                    evt.EventPoints.Add(new EventPoint
                    {
                        EventId = AddPointsEventIdForm.Value,
                        PointId = pointId
                    });
                }
            }

            _context.Events.Update(evt);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        // Odebrání stanoviště z akce
        public async Task<IActionResult> OnPostRemovePointAsync()
        {
            var eventPoint = await _context.EventPoints
                .FirstOrDefaultAsync(ep => ep.EventId == RemoveEventId && ep.PointId == RemovePointId);

            if (eventPoint != null)
            {
                _context.EventPoints.Remove(eventPoint);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }

    public class PointDto
    {
        public string PointId { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string SpecializationName { get; set; } = string.Empty;
    }
}