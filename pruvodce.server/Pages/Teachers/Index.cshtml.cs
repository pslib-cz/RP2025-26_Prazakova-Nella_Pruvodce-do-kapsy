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

        public List<Teacher> Teachers { get; set; } = new();

        public string? Search { get; set; }

        public async Task OnGetAsync(string? search)
        {
            Search = search?.Trim().ToLower();

            var query = _context.Teachers.Include(t => t.Note).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var searchValue = Search;
                query = query.Where(t =>
                    (t.FirstN ?? "").ToLower().Contains(Search) ||
                    (t.LastN ?? "").ToLower().Contains(Search) ||
                    (t.Note == null ? "" : t.Note.Text).ToLower().Contains(searchValue) ||
                    (t.Degree ?? "").ToLower().Contains(Search));
            }

            Teachers = await query
                .OrderBy(t => t.LastN)
                .ThenBy(t => t.FirstN)
                .ToListAsync();
        }
    }
}