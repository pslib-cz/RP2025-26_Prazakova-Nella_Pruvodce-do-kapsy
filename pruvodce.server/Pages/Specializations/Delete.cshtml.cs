using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Specializations
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Specialization? Specialization { get; set; }

        public int RelatedPointsCount { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
                return NotFound();

            Specialization = await _context.Specializations
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SpecializationId == id);

            if (Specialization == null)
                return NotFound();

            RelatedPointsCount = await _context.Points
                .CountAsync(p => p.SpecializationId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Specialization == null)
                return NotFound();

            var entity = await _context.Specializations
                .FirstOrDefaultAsync(s => s.SpecializationId == Specialization.SpecializationId);

            if (entity == null)
                return NotFound();

            var relatedPoints = await _context.Points
                .Where(p => p.SpecializationId == entity.SpecializationId)
                .ToListAsync();

            foreach (var p in relatedPoints)
            {
                p.SpecializationId = null;
            }

            _context.Specializations.Remove(entity);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}