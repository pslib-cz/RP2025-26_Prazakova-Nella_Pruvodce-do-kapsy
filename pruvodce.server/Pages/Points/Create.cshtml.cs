using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;
using pruvodce.server.Services;

namespace pruvodce.server.Pages.Points
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
        public Point Point { get; set; } = new()
        {
            Label = string.Empty
        };

        [BindProperty]
        public List<string> SelectedTeacherIds { get; set; } = new();

        [BindProperty]
        public List<string> SelectedSubjectIds { get; set; } = new();

        public SelectList RoomItems { get; set; } = default!;
        public MultiSelectList TeacherItems { get; set; } = default!;
        public MultiSelectList SubjectItems { get; set; } = default!;
        public SelectList EventItems { get; set; } = default!;
        public SelectList SpecializationItems { get; set; } = default!;

        public async Task OnGetAsync()
        {
            await LoadSelectListsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Point.PointId))
            {
                Point.PointId = Guid.NewGuid().ToString();
            }

            ModelState.Remove("Point.PointId");
            ModelState.Remove("Point.Subjects");
            ModelState.Remove("Point.Teachers");
            ModelState.Remove("Point.Event");
            ModelState.Remove("Point.Specialization");


            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var roomExists = await RoomExistsAsync(Point.RoomId);

            if (!roomExists)
            {
                ModelState.AddModelError("Point.RoomId", "Vyberte existující místnost.");
                await LoadSelectListsAsync();
                return Page();
            }

            Point.Teachers = await _context.Teachers
                .Where(t => SelectedTeacherIds.Contains(t.TeacherId))
                .ToListAsync();

            Point.Subjects = await _context.Subjects
                .Where(s => SelectedSubjectIds.Contains(s.SubjectId))
                .ToListAsync();

            _context.Points.Add(Point);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadSelectListsAsync()
        {
            await LoadRoomItemsAsync();

            var teachers = await _context.Teachers
                .OrderBy(t => t.LastN)
                .ThenBy(t => t.FirstN)
                .ToListAsync();

            var teacherItems = teachers
                .Select(t => new
                {
                    t.TeacherId,
                    FullName = $"{t.FirstN} {t.LastN}"
                })
                .ToList();

            TeacherItems = new MultiSelectList(
                teacherItems,
                "TeacherId",
                "FullName",
                SelectedTeacherIds
            );

            var subjects = await _context.Subjects
                .OrderBy(s => s.Name)
                .ToListAsync();

            SubjectItems = new MultiSelectList(
                subjects,
                "SubjectId",
                "Name",
                SelectedSubjectIds
            );

            var events = await _context.Events
                .OrderBy(e => e.StartDate)
                .ToListAsync();

            EventItems = new SelectList(
                events,
                "EventId",
                "Name",
                Point.EventId
            );

            var specializations = await _context.Specializations
                .OrderBy(s => s.Name)
                .ToListAsync();

            SpecializationItems = new SelectList(
                specializations,
                "SpecializationId",
                "Name",
                Point.SpecializationId
            );
        }

        private async Task LoadRoomItemsAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();

            var rooms = mapData.Buildings
                .SelectMany(b => b.Floors.SelectMany(f => f.Rooms.Select(r => new
                {
                    r.RoomId,
                    DisplayName = $"{b.Name} / {f.Name} / {r.Label}"
                })))
                .OrderBy(r => r.DisplayName)
                .ToList();

            RoomItems = new SelectList(
                rooms,
                "RoomId",
                "DisplayName",
                Point.RoomId
            );
        }

        private async Task<bool> RoomExistsAsync(string? roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                return false;
            }

            var mapData = await _mapDataService.GetMapDataAsync();

            return mapData.Buildings
                .SelectMany(b => b.Floors)
                .SelectMany(f => f.Rooms)
                .Any(r => r.RoomId == roomId);
        }
    }
}