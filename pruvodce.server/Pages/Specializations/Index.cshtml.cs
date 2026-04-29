using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Specializations
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Specialization> Specializations { get; set; } = new List<Specialization>();

        public async Task OnGetAsync()
        {
            Specializations = await _context.Specializations.AsNoTracking().ToListAsync();
        }
    }
}