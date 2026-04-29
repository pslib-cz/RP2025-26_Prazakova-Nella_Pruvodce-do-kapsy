using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public Event Event { get; set; } = default!;

        public SelectList BuildingItems { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Event = new Event
            {
                Name = string.Empty,
                StartDate = RoundToMinute(DateTime.Now),
                EndDate = RoundToMinute(DateTime.Now.AddHours(2)),
                IsActive = true
            };

            await LoadSelectListsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var mapData = await _mapDataService.GetMapDataAsync();

            var buildingExists = mapData.Buildings
                .Any(b => b.BuildingId == Event.BuildingId);

            if (!buildingExists)
            {
                ModelState.AddModelError("Event.BuildingId", "Vyberte existující budovu.");
                await LoadSelectListsAsync();
                return Page();
            }

            if (Event.EndDate <= Event.StartDate)
            {
                ModelState.AddModelError("Event.EndDate", "Konec akce musí být pozdìji než zaèátek.");
                await LoadSelectListsAsync();
                return Page();
            }

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadSelectListsAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();

            BuildingItems = new SelectList(
                mapData.Buildings.OrderBy(b => b.Name),
                "BuildingId",
                "Name"
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