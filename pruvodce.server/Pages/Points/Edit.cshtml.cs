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
        public StudentNote Note { get; set; } = new()
        {
            Text = string.Empty,
            StudentName = "Student"
        };

        [BindProperty]
        public List<string> SelectedTeacherIds { get; set; } = new();

        [BindProperty]
        public List<string> SelectedSubjectIds { get; set; } = new();

        public SelectList RoomItems { get; set; } = default!;
        public SelectList EventItems { get; set; } = default!;
        public SelectList SpecializationItems { get; set; } = default!;
        public SelectList NoteFieldItems { get; set; } = default!;
        public MultiSelectList TeacherItems { get; set; } = default!;
        public MultiSelectList SubjectItems { get; set; } = default!;

        public List<SelectListItem> IconItems { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var point = await _context.Points
                .Include(p => p.Teachers)
                .Include(p => p.PointSubjects)
                .Include(p => p.Note)
                .FirstOrDefaultAsync(p => p.PointId == id);

            if (point == null)
            {
                return NotFound();
            }

            Point = point;

            if (Point.Icon == null)
            {
                Point.Icon = PointIcon.Jine;
            }

            Note = point.Note ?? new StudentNote
            {
                Text = string.Empty,
                StudentName = "Student"
            };

            SelectedTeacherIds = point.Teachers
                .Select(t => t.TeacherId)
                .ToList();

            SelectedSubjectIds = point.PointSubjects
                .Select(s => s.SubjectId)
                .ToList();

            await LoadSelectListsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Point.Teachers");
            ModelState.Remove("Point.PointSubjects");
            ModelState.Remove("Point.Event");
            ModelState.Remove("Point.Specialization");
            ModelState.Remove("Point.Note");

            bool noteHasAnyValue =
                !string.IsNullOrWhiteSpace(Note.Text) ||
                (!string.IsNullOrWhiteSpace(Note.StudentName) && Note.StudentName != "Student") ||
                Note.StudentField != null;

            ModelState.Remove("Note.Text");
            ModelState.Remove("Note.StudentName");
            ModelState.Remove("Note.StudentField");
            ModelState.Remove("Note.StudentYear");

            if (noteHasAnyValue)
            {
                if (string.IsNullOrWhiteSpace(Note.Text))
                {
                    ModelState.AddModelError("Note.Text", "Poznámka je povinná.");
                }

                if (Note.StudentField == null)
                {
                    ModelState.AddModelError("Note.StudentField", "Obor je povinný.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var existing = await _context.Points
                .Include(p => p.Teachers)
                .Include(p => p.PointSubjects)
                .Include(p => p.Note)
                .FirstOrDefaultAsync(p => p.PointId == Point.PointId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Label = Point.Label?.Trim() ?? string.Empty;
            existing.Description = Point.Description;
            existing.Icon = Point.Icon ?? PointIcon.Jine;
            existing.RoomId = Point.RoomId;
            existing.EventId = Point.EventId;
            existing.SpecializationId = Point.SpecializationId;

            existing.Teachers.Clear();

            var teachers = await _context.Teachers
                .Where(t => SelectedTeacherIds.Contains(t.TeacherId))
                .ToListAsync();

            foreach (var teacher in teachers)
            {
                existing.Teachers.Add(teacher);
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

            if (noteHasAnyValue)
            {
                if (existing.Note == null)
                {
                    existing.Note = new StudentNote
                    {
                        StudentNoteId = Guid.NewGuid().ToString()
                    };
                }

                existing.Note.Text = Note.Text?.Trim() ?? string.Empty;
                existing.Note.StudentName = string.IsNullOrWhiteSpace(Note.StudentName)
                    ? "Student"
                    : Note.StudentName.Trim();
                existing.Note.StudentField = Note.StudentField;
            }
            else
            {
                if (existing.Note != null)
                {
                    _context.Remove(existing.Note);
                }

                existing.Note = null;
                existing.NoteId = null;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadSelectListsAsync()
        {
            await LoadRoomsAsync();

            EventItems = new SelectList(
                await _context.Events
                    .AsNoTracking()
                    .OrderBy(e => e.Name)
                    .ToListAsync(),
                "EventId",
                "Name",
                Point.EventId
            );

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

            NoteFieldItems = new SelectList(
                Enum.GetValues<FieldType>()
                    .Select(field => new SelectListItem
                    {
                        Value = field.ToString(),
                        Text = GetFieldTypeLabel(field)
                    }),
                "Value",
                "Text",
                Note.StudentField
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