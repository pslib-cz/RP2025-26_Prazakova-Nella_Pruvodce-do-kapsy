using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Teachers
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
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

        public void OnGet()
        {
            LoadNoteFields();
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

            if (string.IsNullOrEmpty(Teacher.TeacherId))
                Teacher.TeacherId = Guid.NewGuid().ToString();

            if (noteHasAnyValue)
            {
                Note.StudentNoteId = Guid.NewGuid().ToString();
                Note.StudentName = string.IsNullOrWhiteSpace(Note.StudentName)
                    ? "Student"
                    : Note.StudentName.Trim();

                Teacher.Note = Note;
                Teacher.NoteId = Note.StudentNoteId;
            }
            else
            {
                Teacher.Note = null;
                Teacher.NoteId = null;
            }

            _context.Teachers.Add(Teacher);
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