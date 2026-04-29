using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;
using pruvodce.server.Services;

namespace pruvodce.server.Pages.Points
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
        public SelectList EventItems { get; set; } = default!;
        public MultiSelectList SubjectItems { get; set; } = default!;
        public SelectList SpecializationItems { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var entity = await _context.Points
                .Include(p => p.Teachers)
                .Include(p => p.Subjects)
                .Include(p => p.Event)
                .Include(p => p.Specialization)
                .FirstOrDefaultAsync(p => p.PointId == id);

            if (entity == null)
            {
                return NotFound();
            }

            Point = entity;

            SelectedTeacherIds = Point.Teachers
                .Select(t => t.TeacherId)
                .ToList();

            SelectedSubjectIds = Point.Subjects
                .Select(s => s.SubjectId)
                .ToList();

            await LoadSelectListsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Point.Teachers");
            ModelState.Remove("Point.Subjects");
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

            var existing = await _context.Points
                .Include(p => p.Teachers)
                .Include(p => p.Subjects)
                .FirstOrDefaultAsync(p => p.PointId == Point.PointId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Label = Point.Label;
            existing.Description = Point.Description;
            existing.RoomId = Point.RoomId;
            existing.EventId = Point.EventId;
            existing.Note = Point.Note;
            existing.Icon = Point.Icon;
            existing.SpecializationId = Point.SpecializationId;

            existing.Teachers.Clear();

            var selectedTeachers = await _context.Teachers
                .Where(t => SelectedTeacherIds.Contains(t.TeacherId))
                .ToListAsync();

            foreach (var teacher in selectedTeachers)
            {
                existing.Teachers.Add(teacher);
            }

            existing.Subjects.Clear();

            var selectedSubjects = await _context.Subjects
                .Where(s => SelectedSubjectIds.Contains(s.SubjectId))
                .ToListAsync();

            foreach (var subject in selectedSubjects)
            {
                existing.Subjects.Add(subject);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadSelectListsAsync()
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
                .OrderBy(e => e.Name)
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