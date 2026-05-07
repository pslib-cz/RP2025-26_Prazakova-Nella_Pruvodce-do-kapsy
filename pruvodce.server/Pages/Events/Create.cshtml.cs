using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;
using pruvodce.server.Services;

namespace pruvodce.server.Pages.Events
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly MapDataService _mapDataService;

        public CreateModel(ApplicationDbContext context, MapDataService mapDataService)
        {
            _context = context;
            _mapDataService = mapDataService;
        }

        [BindProperty]
        public Event Event { get; set; } = new()
        {
            Name = string.Empty
        };

        [BindProperty]
        public List<int> SelectedBuildingIds { get; set; } = new();

        public MultiSelectList BuildingItems { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Event = new Event
            {
                Name = string.Empty,
                StartDate = RoundToMinute(DateTime.Now),
                EndDate = RoundToMinute(DateTime.Now.AddHours(2)),
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await LoadSelectListsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var savedEventId = await SaveEventAsync();

            if (savedEventId == null)
            {
                return Page();
            }

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostSaveAndCreatePointAsync()
        {
            var savedEventId = await SaveEventAsync();

            if (savedEventId == null)
            {
                return Page();
            }

            return RedirectToPage("/Points/Create", new { eventId = savedEventId.Value });
        }

        private async Task<int?> SaveEventAsync()
        {
            ModelState.Remove("Event.Points");
            ModelState.Remove("Event.EventBuildings");

            Event.Name = Event.Name?.Trim() ?? string.Empty;

            if (SelectedBuildingIds == null || SelectedBuildingIds.Count == 0)
            {
                ModelState.AddModelError("SelectedBuildingIds", "Vyberte alespoň jednu budovu.");
            }

            if (Event.EndDate <= Event.StartDate)
            {
                ModelState.AddModelError("Event.EndDate", "Konec akce musí být později než začátek.");
            }

            if (!string.IsNullOrWhiteSpace(Event.Name))
            {
                var eventNameExists = await _context.Events
                    .AnyAsync(e => e.Name.ToLower() == Event.Name.ToLower());

                if (eventNameExists)
                {
                    ModelState.AddModelError("Event.Name", "Akce s tímto názvem už existuje.");
                }
            }

            var mapData = await _mapDataService.GetMapDataAsync();

            var validBuildingIds = mapData.Buildings
                .Select(b => b.BuildingId)
                .ToHashSet();

            if (SelectedBuildingIds != null &&
                SelectedBuildingIds.Any(id => !validBuildingIds.Contains(id)))
            {
                ModelState.AddModelError("SelectedBuildingIds", "Vyberte existující budovy.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return null;
            }

            var selectedBuildingIds = SelectedBuildingIds!
                .Distinct()
                .ToList();

            if (Event.IsActive)
            {
                var otherEvents = await _context.Events
                    .Include(e => e.EventBuildings)
                    .Where(e => e.EventBuildings.Any(eb => selectedBuildingIds.Contains(eb.BuildingId)))
                    .ToListAsync();

                foreach (var otherEvent in otherEvents)
                {
                    otherEvent.IsActive = false;
                }
            }

            Event.CreatedAt = DateTime.Now;

            Event.EventBuildings = selectedBuildingIds
                .Select(buildingId => new EventBuilding
                {
                    BuildingId = buildingId
                })
                .ToList();

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            return Event.EventId;
        }

        private async Task LoadSelectListsAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();

            BuildingItems = new MultiSelectList(
                mapData.Buildings.OrderBy(b => b.Name),
                "BuildingId",
                "Name",
                SelectedBuildingIds
            );
        }

        private static DateTime RoundToMinute(DateTime dateTime)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                0
            );
        }
    }
}