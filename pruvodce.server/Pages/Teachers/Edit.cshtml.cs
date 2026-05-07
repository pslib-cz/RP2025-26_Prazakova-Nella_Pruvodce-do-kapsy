using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Teachers
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = default!;

        [BindProperty]
        public StudentNote Note { get; set; } = new()
        {
            Text = string.Empty,
            StudentName = "Student"
        };

        public SelectList NoteFieldItems { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var item = await _context.Teachers
                .Include(t => t.Note)
                .FirstOrDefaultAsync(t => t.TeacherId == id);

            if (item == null)
                return NotFound();

            Teacher = item;

            Note = item.Note ?? new StudentNote
            {
                Text = string.Empty,
                StudentName = "Student"
            };

            LoadNoteFields();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Teacher.Note");

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
                LoadNoteFields();
                return Page();
            }

            var existing = await _context.Teachers
                .Include(t => t.Note)
                .FirstOrDefaultAsync(t => t.TeacherId == Teacher.TeacherId);

            if (existing == null)
                return NotFound();

            existing.FirstN = Teacher.FirstN;
            existing.LastN = Teacher.LastN;
            existing.Degree = Teacher.Degree;

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
                existing.Note.StudentYear = null;
            }
            else
            {
                if (existing.Note != null)
                    _context.Remove(existing.Note);

                existing.Note = null;
                existing.NoteId = null;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private void LoadNoteFields()
        {
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