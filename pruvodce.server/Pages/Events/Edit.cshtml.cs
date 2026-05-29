using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;
using pruvodce.server.Services;

namespace pruvodce.server.Pages.Events
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly MapDataService _mapDataService;

        public EditModel(ApplicationDbContext context, MapDataService mapDataService)
        {
            _context = context;
            _mapDataService = mapDataService;
        }

        [BindProperty]
        public Event Event { get; set; } = default!;

        [BindProperty]
        public List<int> SelectedBuildingIds { get; set; } = new();

        public MultiSelectList BuildingItems { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Events
                .Include(e => e.EventBuildings)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventId == id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            Event = entity;

            SelectedBuildingIds = Event.EventBuildings
                .Select(eb => eb.BuildingId)
                .ToList();

            await LoadSelectListsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Event.Points");
            ModelState.Remove("Event.EventBuildings");

            if (SelectedBuildingIds == null || SelectedBuildingIds.Count == 0)
            {
                ModelState.AddModelError("SelectedBuildingIds", "Vyberte alespoň jednu budovu.");
            }

            var mapData = await _mapDataService.GetMapDataAsync();

            var validBuildingIds = mapData.Buildings
                .Select(b => b.BuildingId)
                .ToHashSet();

            if (SelectedBuildingIds!.Any(id => !validBuildingIds.Contains(id)))
            {
                ModelState.AddModelError("SelectedBuildingIds", "Vyberte existující budovy.");
            }

            Event.Name = Event.Name?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(Event.Name))
            {
                var eventNameExists = await _context.Events
                    .AnyAsync(e =>
                        e.EventId != Event.EventId &&
                        e.Name.ToLower() == Event.Name.ToLower());

                if (eventNameExists)
                {
                    ModelState.AddModelError("Event.Name", "Akce s tímto názvem už existuje.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var selectedBuildingIds = SelectedBuildingIds
                .Distinct()
                .ToList();

            var existing = await _context.Events
                .Include(e => e.EventBuildings)
                .FirstOrDefaultAsync(e => e.EventId == Event.EventId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = Event.Name;
            existing.StartDate = Event.StartDate;
            existing.EndDate = Event.EndDate;
            existing.Description = Event.Description;
            // IsActive se nemění — řídí se pouze přes Events/Index

            existing.EventBuildings.Clear();

            foreach (var buildingId in selectedBuildingIds)
            {
                existing.EventBuildings.Add(new EventBuilding
                {
                    EventId = existing.EventId,
                    BuildingId = buildingId
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
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
    }
}