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
            PointId = string.Empty,
            Label = string.Empty,
            Icon = PointIcon.Jine,
            AreStudents = false,
            SpecializationId = string.Empty
        };

        [BindProperty]
        public List<string> SelectedTeacherIds { get; set; } = new();

        [BindProperty]
        public List<string> SelectedSubjectIds { get; set; } = new();

        public SelectList RoomItems { get; set; } = default!;
        public MultiSelectList TeacherItems { get; set; } = default!;
        public MultiSelectList SubjectItems { get; set; } = default!;
        public SelectList SpecializationItems { get; set; } = default!;
        public List<SelectListItem> IconItems { get; set; } = new();

        public async Task OnGetAsync()
        {
            Point.Icon = PointIcon.Jine;
            await LoadSelectListsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Point.PointId ??= Guid.NewGuid().ToString();

            ModelState.Remove("Point.PointId");
            ModelState.Remove("Point.Teachers");
            ModelState.Remove("Point.PointSubjects");
            ModelState.Remove("Point.Specialization");
            ModelState.Remove("Point.EventPoints");

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

            if (!await SpecializationExistsAsync(Point.SpecializationId))
            {
                ModelState.AddModelError("Point.SpecializationId", "Vyberte existující zaměření.");
                await LoadSelectListsAsync();
                return Page();
            }

            if (SelectedTeacherIds.Any())
            {
                var validTeacherIds = await _context.Teachers
                    .Where(t => SelectedTeacherIds.Contains(t.TeacherId))
                    .Select(t => t.TeacherId)
                    .ToListAsync();

                var invalidIds = SelectedTeacherIds.Except(validTeacherIds).ToList();
                if (invalidIds.Any())
                {
                    ModelState.AddModelError("SelectedTeacherIds", "Někteří vybraní učitelé neexistují.");
                    await LoadSelectListsAsync();
                    return Page();
                }
            }

            if (SelectedSubjectIds.Any())
            {
                var validSubjectIds = await _context.Subjects
                    .Where(s => SelectedSubjectIds.Contains(s.SubjectId))
                    .Select(s => s.SubjectId)
                    .ToListAsync();

                var invalidIds = SelectedSubjectIds.Except(validSubjectIds).ToList();
                if (invalidIds.Any())
                {
                    ModelState.AddModelError("SelectedSubjectIds", "Některé vybrané předměty neexistují.");
                    await LoadSelectListsAsync();
                    return Page();
                }
            }

            Point.Label = Point.Label.Trim();

            if (!string.IsNullOrWhiteSpace(Point.Description))
            {
                Point.Description = Point.Description.Trim();
            }

            Point.PointTeachers = SelectedTeacherIds
                .Select(id => new PointTeacher
                {
                    PointTeacherId = Guid.NewGuid().ToString(),
                    PointId = Point.PointId,
                    TeacherId = id
                })
                .ToList();

            Point.PointSubjects = SelectedSubjectIds
                .Select(id => new PointSubject
                {
                    PointId = Point.PointId,
                    SubjectId = id
                })
                .ToList();

            _context.Points.Add(Point);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task<bool> SpecializationExistsAsync(string? specializationId)
        {
            if (string.IsNullOrWhiteSpace(specializationId))
                return false;

            return await _context.Specializations.AnyAsync(s => s.SpecializationId == specializationId);
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

            SpecializationItems = new SelectList(
                await _context.Specializations
                    .AsNoTracking()
                    .OrderBy(s => s.Name)
                    .ToListAsync(),
                "SpecializationId",
                "Name",
                Point.SpecializationId
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
