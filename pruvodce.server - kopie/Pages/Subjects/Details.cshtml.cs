using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Subject? Subject { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            Subject = await _context.Subjects
                .Include(s => s.PointSubjects)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SubjectId == id);

            if (Subject == null)
                return NotFound();

            return Page();
        }
    }
}