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

        public async Task<IActionResult> OnPostAsync()
        {
            if (Specialization == null || string.IsNullOrEmpty(Specialization.SpecializationId))
                return NotFound();

            var entity = await _context.Specializations.FindAsync(Specialization.SpecializationId);
            if (entity != null)
            {
                _context.Specializations.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}