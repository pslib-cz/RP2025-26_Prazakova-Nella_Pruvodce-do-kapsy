using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Subjects
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subject? Subject { get; set; }

        public int RelatedPointsCount { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            Subject = await _context.Subjects
                .Include(s => s.PointSubjects)
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (Subject == null)
                return NotFound();

            RelatedPointsCount = Subject.PointSubjects.Count;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Subject == null)
                return NotFound();

            var entity = await _context.Subjects
                .Include(s => s.PointSubjects)
                .FirstOrDefaultAsync(s => s.SubjectId == Subject.SubjectId);

            if (entity == null)
                return NotFound();

            _context.PointSubjects.RemoveRange(entity.PointSubjects);

            _context.Subjects.Remove(entity);

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}