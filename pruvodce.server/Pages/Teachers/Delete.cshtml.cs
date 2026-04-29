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

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            Teacher = await _context.Teachers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.TeacherId == id);

            if (Teacher == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Teacher == null || string.IsNullOrEmpty(Teacher.TeacherId))
                return NotFound();

            var entity = await _context.Teachers.FindAsync(Teacher.TeacherId);
            if (entity != null)
            {
                _context.Teachers.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}