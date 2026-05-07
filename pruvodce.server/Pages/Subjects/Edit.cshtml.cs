using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Subjects
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subject Subject { get; set; } = default!;

        [BindProperty]
        public StudentNote Note { get; set; } = new()
        {
            Text = string.Empty,
            StudentName = "Student"
        };

        [BindProperty]
        public List<string> SelectedPointIds { get; set; } = new();

        public MultiSelectList PointItems { get; set; } = default!;
        public SelectList NoteFieldItems { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var item = await _context.Subjects
                .Include(s => s.Note)
                .Include(s => s.PointSubjects)
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (item == null)
                return NotFound();

            Subject = item;

            Note = item.Note ?? new StudentNote
            {
                Text = string.Empty,
                StudentName = "Student"
            };

            SelectedPointIds = Subject.PointSubjects
                .Select(p => p.PointId)
                .ToList();

            await LoadSelectListsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Subject.Note");
            ModelState.Remove("Subject.PointSubjects");

            var noteHasAnyValue =
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
                    ModelState.AddModelError("Note.Text", "Poznámka je povinná.");

                if (Note.StudentField == null)
                    ModelState.AddModelError("Note.StudentField", "Obor je povinný.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var subjectToUpdate = await _context.Subjects
                .Include(s => s.Note)
                .Include(s => s.PointSubjects)
                .FirstOrDefaultAsync(s => s.SubjectId == Subject.SubjectId);

            if (subjectToUpdate == null)
                return NotFound();

            subjectToUpdate.Name = Subject.Name;
            subjectToUpdate.Acronym = Subject.Acronym;

            if (noteHasAnyValue)
            {
                if (subjectToUpdate.Note == null)
                {
                    subjectToUpdate.Note = new StudentNote
                    {
                        StudentNoteId = Guid.NewGuid().ToString()
                    };
                }

                subjectToUpdate.Note.Text = Note.Text?.Trim() ?? string.Empty;
                subjectToUpdate.Note.StudentName = string.IsNullOrWhiteSpace(Note.StudentName)
                    ? "Student"
                    : Note.StudentName.Trim();
                subjectToUpdate.Note.StudentField = Note.StudentField;
                subjectToUpdate.Note.StudentYear = null;
            }
            else
            {
                if (subjectToUpdate.Note != null)
                    _context.Remove(subjectToUpdate.Note);

                subjectToUpdate.Note = null;
                subjectToUpdate.NoteId = null;
            }

            _context.PointSubjects.RemoveRange(subjectToUpdate.PointSubjects);

            var newPointSubjects = SelectedPointIds
                .Select(pointId => new PointSubject
                {
                    PointId = pointId,
                    SubjectId = subjectToUpdate.SubjectId
                })
                .ToList();

            await _context.PointSubjects.AddRangeAsync(newPointSubjects);

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadSelectListsAsync()
        {
            var points = await _context.Points
                .AsNoTracking()
                .OrderBy(p => p.Label)
                .Select(p => new
                {
                    p.PointId,
                    p.Label
                })
                .ToListAsync();

            PointItems = new MultiSelectList(
                points,
                "PointId",
                "Label",
                SelectedPointIds
            );

            NoteFieldItems = new SelectList(
                Enum.GetValues<FieldType>()
                    .Select(field => new SelectListItem
                    {
                        Value = field.ToString(),
                        Text = field.ToString()
                    }),
                "Value",
                "Text",
                Note.StudentField
            );
        }
    }
}