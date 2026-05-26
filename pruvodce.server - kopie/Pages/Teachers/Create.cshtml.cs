using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public List<string> SelectedNoteIds { get; set; } = new();

        public MultiSelectList AvailableNotes { get; set; } = default!;

        public async Task OnGetAsync()
        {
            await LoadAvailableNotesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // =========================
            // VALIDACE MAX 3
            // =========================
            if (SelectedNoteIds.Count > 3)
            {
                ModelState.AddModelError(
                    "SelectedNoteIds",
                    "Můžete vybrat maximálně 3 poznámky."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadAvailableNotesAsync();
                return Page();
            }

            // =========================
            // CREATE TEACHER ID
            // =========================
            if (string.IsNullOrEmpty(Teacher.TeacherId))
                Teacher.TeacherId = Guid.NewGuid().ToString();

            _context.Teachers.Add(Teacher);

            await _context.SaveChangesAsync();

            // =========================
            // PŘIŘAZENÍ POZNÁMEK
            // (one-to-many)
            // =========================
            if (SelectedNoteIds.Any())
            {
                var notes = await _context.StudentNotes
                    .Where(n => SelectedNoteIds.Contains(n.StudentNoteId))
                    .ToListAsync();

                foreach (var note in notes)
                {
                    note.TeacherId = Teacher.TeacherId;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }

        private async Task LoadAvailableNotesAsync()
        {
            var availableNotes = await _context.StudentNotes
                .Where(n => n.TargetType == "TEACHER")
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            AvailableNotes = new MultiSelectList(
                availableNotes,
                "StudentNoteId",
                "Text",
                SelectedNoteIds
            );
        }
    }
}