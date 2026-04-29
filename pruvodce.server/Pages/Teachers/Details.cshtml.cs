using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Teachers
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Teacher? Teacher { get; set; }
        public IList<Subject>? Subjects { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            Teacher = await _context.Teachers
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TeacherId == id);

            if (Teacher == null)
                return NotFound();

            Subjects = await _context.Subjects
                .AsNoTracking()
                .ToListAsync();

            return Page();
        }
    }
}