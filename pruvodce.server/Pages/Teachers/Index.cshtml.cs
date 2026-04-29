using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Teacher> Teachers { get; set; } = new List<Teacher>();

        public async Task OnGetAsync()
        {
            Teachers = await _context.Teachers
                .AsNoTracking()
                .ToListAsync();
        }
    }
}