using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Points
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Point> Points { get; set; } = new List<Point>();

        public async Task OnGetAsync()
        {
            Points = await _context.Points
                .Include(p => p.Event)
                .Include(p => p.Specialization)
                .Include(p => p.Teachers)
                .Include(p => p.Subjects)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}