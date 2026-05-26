using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Points
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Point? Point { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            Point = await _context.Points
                .Include(p => p.PointSubjects)
                    .ThenInclude(ps => ps.Subject)
                .Include(p => p.PointTeachers)
                    .ThenInclude(pt => pt.Teacher)
                .Include(p => p.Specialization)
                .FirstOrDefaultAsync(p => p.PointId == id);

            if (Point == null)
                return NotFound();

            return Page();
        }
    }
}
