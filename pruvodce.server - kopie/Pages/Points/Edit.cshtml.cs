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
        public Point Point { get; set; } = default!;

        [BindProperty]
        public List<string> SelectedTeacherIds { get; set; } = new();

        [BindProperty]
        public List<string> SelectedSubjectIds { get; set; } = new();

        public SelectList RoomItems { get; set; } = default!;
        public SelectList SpecializationItems { get; set; } = default!;
        public MultiSelectList TeacherItems { get; set; } = default!;
        public MultiSelectList SubjectItems { get; set; } = default!;
        public List<SelectListItem> IconItems { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var point = await _context.Points
                .Include(p => p.PointTeachers)
                    .ThenInclude(pt => pt.Teacher)
                .Include(p => p.PointSubjects)
                .FirstOrDefaultAsync(p => p.PointId == id);

            if (point == null)
            {
                return NotFound();
            }

            Point = point;
            Point.Icon = point.Icon;

            SelectedTeacherIds = point.PointTeachers
                .Select(pt => pt.TeacherId)
                .ToList();

            SelectedSubjectIds = point.PointSubjects
                .Select(s => s.SubjectId)
                .ToList();

            await LoadSelectListsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Point.PointTeachers");
            ModelState.Remove("Point.PointSubjects");
            ModelState.Remove("Point.Specialization");
            ModelState.Remove("Point.EventPoints");

    

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var existing = await _context.Points
                .Include(p => p.PointTeachers)
                    .ThenInclude(pt => pt.Teacher)
                .Include(p => p.PointSubjects)
                .FirstOrDefaultAsync(p => p.PointId == Point.PointId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Label = Point.Label?.Trim() ?? string.Empty;
            existing.Description = Point.Description;
            existing.Icon = Point.Icon;
            existing.RoomId = Point.RoomId;
            existing.SpecializationId = Point.SpecializationId;
            existing.AreStudents = Point.AreStudents;

            existing.PointTeachers.Clear();

            var teachers = await _context.Teachers
                .Where(t => SelectedTeacherIds.Contains(t.TeacherId))
                .ToListAsync();

            foreach (var teacher in teachers)
            {
                existing.PointTeachers.Add(new PointTeacher
                {
                    PointId = existing.PointId,
                    TeacherId = teacher.TeacherId
                });
            }

            _context.PointSubjects.RemoveRange(existing.PointSubjects);

            var subjects = SelectedSubjectIds
                .Select(id => new PointSubject
                {
                    PointId = existing.PointId,
                    SubjectId = id
                })
                .ToList();

            await _context.PointSubjects.AddRangeAsync(subjects);

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadSelectListsAsync()
        {
            await LoadRoomsAsync();

            SpecializationItems = new SelectList(
                await _context.Specializations
                    .AsNoTracking()
                    .OrderBy(s => s.Name)
                    .ToListAsync(),
                "SpecializationId",
                "Name",
                Point.SpecializationId
            );

            var teachers = await _context.Teachers
                .AsNoTracking()
                .OrderBy(t => t.LastN)
                .ThenBy(t => t.FirstN)
                .Select(t => new
                {
                    t.TeacherId,
                    FullName = string.IsNullOrWhiteSpace(t.Degree)
                        ? $"{t.FirstN} {t.LastN}"
                        : $"{t.Degree} {t.FirstN} {t.LastN}"
                })
                .ToListAsync();

            TeacherItems = new MultiSelectList(
                teachers,
                "TeacherId",
                "FullName",
                SelectedTeacherIds
            );

            var subjects = await _context.Subjects
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();

            SubjectItems = new MultiSelectList(
                subjects,
                "SubjectId",
                "Name",
                SelectedSubjectIds
            );

            IconItems = Enum.GetValues<PointIcon>()
                .Select(icon => new SelectListItem
                {
                    Value = icon.ToString(),
                    Text = GetPointIconLabel(icon),
                    Selected = Point.Icon == icon
                })
                .ToList();
        }

        private async Task LoadRoomsAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();

            var rooms = mapData.Buildings
                .SelectMany(building => building.Floors.SelectMany(floor =>
                    floor.Rooms.Select(room => new
                    {
                        room.RoomId,
                        Display = $"{building.Name} - {(string.IsNullOrWhiteSpace(room.Label) ? room.RoomId : room.Label)}"
                    })))
                .OrderBy(room => room.Display)
                .ToList();

            RoomItems = new SelectList(
                rooms,
                "RoomId",
                "Display",
                Point.RoomId
            );
        }

        private static string GetPointIconLabel(PointIcon icon)
        {
            return icon switch
            {
                PointIcon.Talk => "Přednáška",
                PointIcon.Hand => "Praktické stanoviště",
                PointIcon.Ucebna => "Ukázka učebny",
                PointIcon.Jine => "Jiné",
                _ => "Jiné"
            };
        }

        private static string GetFieldTypeLabel(FieldType field)
        {
            return field switch
            {
                FieldType.IT => "Informační technologie",
                FieldType.EL => "Elektrotechnika",
                FieldType.ST => "Strojírenství",
                FieldType.TL => "Technické lyceum",
                FieldType.OD => "Oděvnictví",
                FieldType.TE => "Textilnictví",
                _ => field.ToString()
            };
        }
    }
}
