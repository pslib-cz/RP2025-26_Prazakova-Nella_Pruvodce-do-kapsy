using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Subjects
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Subject> Subjects { get; set; } = new();

        public string? Search { get; set; }

        public async Task OnGetAsync(string? search)
        {
            Search = search?.Trim().ToLower();

            var query = _context.Subjects.Include(s => s.Note).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var searchValue = Search;
                query = query.Where(s =>
                    (s.Name ?? "").ToLower().Contains(Search) ||
                    (s.Acronym ?? "").ToLower().Contains(Search) ||
                    (s.Note == null ? "" : s.Note.Text).ToLower().Contains(searchValue));
            }

            Subjects = await query
                .OrderBy(s => s.Name)
                .ToListAsync();
        }
    }
}