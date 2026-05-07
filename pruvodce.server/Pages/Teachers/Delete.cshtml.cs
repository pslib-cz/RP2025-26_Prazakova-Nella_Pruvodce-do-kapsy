using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Teachers
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Teacher? Teacher { get; set; }

        public int RelatedPointsCount { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
                return NotFound();

            Teacher = await _context.Teachers
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TeacherId == id);

            if (Teacher == null)
                return NotFound();

            RelatedPointsCount = await _context.Points
                .Where(p => p.Teachers.Any(t => t.TeacherId == id))
                .CountAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Teacher == null)
                return NotFound();

            var entity = await _context.Teachers
                .FirstOrDefaultAsync(t => t.TeacherId == Teacher.TeacherId);

            if (entity == null)
                return NotFound();

            _context.Teachers.Remove(entity);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}