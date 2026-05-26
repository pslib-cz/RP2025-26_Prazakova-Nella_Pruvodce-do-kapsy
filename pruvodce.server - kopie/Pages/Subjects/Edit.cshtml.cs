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
        public List<string> SelectedPointIds { get; set; } = new();

        [BindProperty]
        public string? SelectedActiveNoteId { get; set; }

        public MultiSelectList PointItems { get; set; } = default!;
        public List<StudentNote> AvailableNotes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            Subject = await _context.Subjects
                .Include(s => s.PointSubjects)
                .Include(s => s.Notes)
                .Include(s => s.ActiveNote)
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (Subject == null)
                return NotFound();

            SelectedActiveNoteId = Subject.ActiveNoteStudentNoteId;

            SelectedPointIds = Subject.PointSubjects
                .Select(p => p.PointId)
                .ToList();

            await LoadSelectListsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var subjectToUpdate = await _context.Subjects
                .Include(s => s.PointSubjects)
                .FirstOrDefaultAsync(s => s.SubjectId == Subject.SubjectId);

            if (subjectToUpdate == null)
                return NotFound();

            subjectToUpdate.Name = Subject.Name;
            subjectToUpdate.Acronym = Subject.Acronym;

            _context.PointSubjects.RemoveRange(subjectToUpdate.PointSubjects);

            var newPoints = SelectedPointIds
                .Select(id => new PointSubject
                {
                    PointId = id,
                    SubjectId = subjectToUpdate.SubjectId
                });

            await _context.PointSubjects.AddRangeAsync(newPoints);

            subjectToUpdate.ActiveNoteStudentNoteId =
                string.IsNullOrWhiteSpace(SelectedActiveNoteId)
                    ? null
                    : SelectedActiveNoteId;

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadSelectListsAsync()
        {
            var points = await _context.Points
                .AsNoTracking()
                .OrderBy(p => p.Label)
                .ToListAsync();

            PointItems = new MultiSelectList(points, "PointId", "Label", SelectedPointIds);

            AvailableNotes = await _context.StudentNotes
                .Where(n => n.TargetType.ToUpper() == "SUBJECT" && n.SubjectId == Subject.SubjectId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}