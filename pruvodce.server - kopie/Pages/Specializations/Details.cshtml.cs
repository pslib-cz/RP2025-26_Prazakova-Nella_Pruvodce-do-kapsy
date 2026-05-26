using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Specializations
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Specialization? Specialization { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            Specialization = await _context.Specializations.AsNoTracking()
                .FirstOrDefaultAsync(m => m.SpecializationId == id);

            if (Specialization == null)
                return NotFound();

            return Page();
        }
    }
}