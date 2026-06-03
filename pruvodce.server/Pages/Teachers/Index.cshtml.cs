using System.Text.Json;
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

            IQueryable<Teacher> query = _context.Teachers
                .Include(t => t.Notes)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var searchValue = Search;

                query = query.Where(t =>
                    (t.FirstN ?? "").ToLower().Contains(searchValue) ||
                    (t.LastN ?? "").ToLower().Contains(searchValue) ||
                    (t.Degree ?? "").ToLower().Contains(searchValue));
            }

            Teachers = await query
                .OrderBy(t => t.LastN)
                .ThenBy(t => t.FirstN)
                .ToListAsync();
        }

        public static int GetSelectedNotesCount(string? selectedNoteIds)
        {
            if (string.IsNullOrEmpty(selectedNoteIds))
                return 0;

            try
            {
                var ids = JsonSerializer.Deserialize<List<string>>(selectedNoteIds);
                return ids?.Count ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}