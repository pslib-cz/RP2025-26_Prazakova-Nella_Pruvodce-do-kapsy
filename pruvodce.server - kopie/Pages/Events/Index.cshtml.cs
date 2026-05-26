using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 3;

        public PagedResult<Event> Events { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (PageNumber < 1)
            {
                PageNumber = 1;
            }

            if (PageSize < 1)
            {
                PageSize = 3;
            }

            var query = _context.Events
                .Include(e => e.EventBuildings)
                .OrderByDescending(e => e.CreatedAt)
                .AsNoTracking();

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            Events = new PagedResult<Event>
            {
                Items = items,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalItems = totalItems
            };
        }
    }
}