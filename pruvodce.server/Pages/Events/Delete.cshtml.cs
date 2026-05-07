using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Events
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Event? Event { get; set; }

        public int RelatedPointsCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventId == id.Value);

            if (Event == null)
            {
                return NotFound();
            }

            RelatedPointsCount = await _context.Points
                .CountAsync(p => p.EventId == Event.EventId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Event == null)
            {
                return NotFound();
            }

            var entity = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == Event.EventId);

            if (entity == null)
            {
                return NotFound();
            }

            var relatedPoints = await _context.Points
                .Where(p => p.EventId == Event.EventId)
                .ToListAsync();

            foreach (var point in relatedPoints)
            {
                point.EventId = null;
            }

            _context.Events.Remove(entity);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}