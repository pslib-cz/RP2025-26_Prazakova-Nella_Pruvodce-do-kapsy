using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Subjects
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subject Subject { get; set; } = default!;

        [BindProperty]
        public string? SelectedNoteId { get; set; }

        public SelectList AvailableNotes { get; set; } = default!;

        public async Task OnGetAsync()
        {
            await LoadAvailableNotesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Subject.Note");

            if (!ModelState.IsValid)
            {
                await LoadAvailableNotesAsync();
                return Page();
            }

            if (string.IsNullOrEmpty(Subject.SubjectId))
                Subject.SubjectId = Guid.NewGuid().ToString();

            _context.Subjects.Add(Subject);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(SelectedNoteId))
            {
                var note = await _context.StudentNotes
                    .FirstOrDefaultAsync(n => n.StudentNoteId == SelectedNoteId);

                if (note != null)
                {
                    note.SubjectId = Subject.SubjectId;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }

        private async Task LoadAvailableNotesAsync()
        {
            var availableNotes = await _context.StudentNotes
                .Where(n => n.TargetType == "SUBJECT")
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            AvailableNotes = new SelectList(
                availableNotes,
                "StudentNoteId",
                "Text",
                SelectedNoteId
            );
        }
    }
}