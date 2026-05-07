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

        public int RelatedPointsCount { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
                return NotFound();

            Point = await _context.Points
                .Include(p => p.Event)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PointId == id);

            if (Point == null)
                return NotFound();

            RelatedPointsCount = await _context.Points
                .CountAsync(p => p.EventId == Point.EventId && p.PointId != Point.PointId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Point == null)
                return NotFound();

            var entity = await _context.Points
                .FirstOrDefaultAsync(p => p.PointId == Point.PointId);

            if (entity == null)
                return NotFound();

            _context.Points.Remove(entity);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}