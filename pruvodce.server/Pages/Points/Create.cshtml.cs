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
            Label = string.Empty,
            Icon = PointIcon.Jine
        };

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
        public MultiSelectList TeacherItems { get; set; } = default!;
        public MultiSelectList SubjectItems { get; set; } = default!;
        public SelectList EventItems { get; set; } = default!;
        public SelectList SpecializationItems { get; set; } = default!;
        public SelectList NoteFieldItems { get; set; } = default!;
        public List<SelectListItem> IconItems { get; set; } = new();

        public async Task OnGetAsync(int? eventId)
        {
            Point.Icon ??= PointIcon.Jine;

            if (eventId.HasValue)
            {
                Point.EventId = eventId.Value;
            }

            await LoadSelectListsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Point.PointId ??= Guid.NewGuid().ToString();
            Point.Icon ??= PointIcon.Jine;

            ModelState.Remove("Point.PointId");
            ModelState.Remove("Point.Teachers");
            ModelState.Remove("Point.PointSubjects");
            ModelState.Remove("Point.Event");
            ModelState.Remove("Point.Specialization");
            ModelState.Remove("Point.Note");

            var noteHasAnyValue =
                !string.IsNullOrWhiteSpace(Note.Text) ||
                (!string.IsNullOrWhiteSpace(Note.StudentName) && Note.StudentName != "Student") ||
                Note.StudentField != null;

            if (!noteHasAnyValue)
            {
                ModelState.Remove("Note.Text");
                ModelState.Remove("Note.StudentName");
                ModelState.Remove("Note.StudentField");
                ModelState.Remove("Note.StudentYear");
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            if (!await RoomExistsAsync(Point.RoomId))
            {
                ModelState.AddModelError("Point.RoomId", "Vyberte existující místnost.");
                await LoadSelectListsAsync();
                return Page();
            }

            Point.Label = Point.Label.Trim();

            if (!string.IsNullOrWhiteSpace(Point.Description))
            {
                Point.Description = Point.Description.Trim();
            }

            Point.Teachers = await _context.Teachers
                .Where(t => SelectedTeacherIds.Contains(t.TeacherId))
                .ToListAsync();

            Point.PointSubjects = SelectedSubjectIds
                .Select(id => new PointSubject
                {
                    PointId = Point.PointId,
                    SubjectId = id
                })
                .ToList();

            if (noteHasAnyValue)
            {
                Note.StudentNoteId = Guid.NewGuid().ToString();
                Note.StudentName = string.IsNullOrWhiteSpace(Note.StudentName)
                    ? "Student"
                    : Note.StudentName.Trim();

                if (!string.IsNullOrWhiteSpace(Note.Text))
                {
                    Note.Text = Note.Text.Trim();
                }

                Point.Note = Note;
                Point.NoteId = Note.StudentNoteId;
            }
            else
            {
                Point.Note = null;
                Point.NoteId = null;
            }

            _context.Points.Add(Point);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadSelectListsAsync()
        {
            await LoadRoomItemsAsync();

            var teachers = await _context.Teachers
                .AsNoTracking()
                .OrderBy(t => t.LastN)
                .ThenBy(t => t.FirstN)
                .Select(t => new
                {
                    t.TeacherId,
                    FullName = $"{t.FirstN} {t.LastN}"
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

        private async Task LoadRoomItemsAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();

            var rooms = mapData.Buildings
                .SelectMany(building => building.Floors.SelectMany(floor =>
                    floor.Rooms.Select(room => new
                    {
                        room.RoomId,
                        DisplayName =
                            $"{building.Name} / {floor.Name} / {(string.IsNullOrWhiteSpace(room.Label) ? room.RoomId : room.Label)}"
                    })))
                .OrderBy(room => room.DisplayName)
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
                .SelectMany(building => building.Floors)
                .SelectMany(floor => floor.Rooms)
                .Any(room => room.RoomId == roomId);
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