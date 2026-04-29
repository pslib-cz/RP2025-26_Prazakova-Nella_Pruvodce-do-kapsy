using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Points
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Point? Point { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Point = await _context.Points
                .Include(p => p.Event)
                .Include(p => p.Specialization)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PointId == id);

            if (Point == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Point == null || string.IsNullOrEmpty(Point.PointId))
            {
                return NotFound();
            }

            var entity = await _context.Points
                .FirstOrDefaultAsync(p => p.PointId == Point.PointId);

            if (entity != null)
            {
                _context.Points.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}