using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Event? Event { get; set; }
        public IList<Point>? Points { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Event = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventId == id.Value);

            if (Event == null)
                return NotFound();

            Points = await _context.Points
                .AsNoTracking()
                .Where(p => p.EventId == Event.EventId)
                .ToListAsync();

            return Page();
        }
    }
}