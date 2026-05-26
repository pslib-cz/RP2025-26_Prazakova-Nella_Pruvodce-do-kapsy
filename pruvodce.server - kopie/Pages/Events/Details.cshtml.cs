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

            // Get points through EventPoint junction
            Points = await _context.EventPoints
                .Where(ep => ep.EventId == Event.EventId)
                .Select(ep => ep.Point!)
                .Where(p => p != null)
                .AsNoTracking()
                .ToListAsync();

            return Page();
        }
    }
}
