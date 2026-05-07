using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public List<Specialization> Specializations { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public FieldType? Type { get; set; }

        public SelectList TypeItems { get; set; } = default!;

        public async Task OnGetAsync()
        {
            LoadTypes();

            var query = _context.Specializations.AsNoTracking();

            if (Type.HasValue)
            {
                query = query.Where(s => s.Type == Type);
            }

            var search = Search?.ToLower();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(s =>
                    (s.Name ?? "").ToLower().Contains(search!) ||
                    (s.Description ?? "").ToLower().Contains(search!) ||
                    (s.Type.HasValue ? s.Type.Value.ToString() : "").ToLower().Contains(search!));
            }

            Specializations = await query
                .OrderBy(s => s.Type)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        private void LoadTypes()
        {
            TypeItems = new SelectList(
                Enum.GetValues<FieldType>()
                    .Cast<FieldType>()
                    .Select(t => new SelectListItem
                    {
                        Value = t.ToString(),
                        Text = t.ToString()
                    }),
                "Value",
                "Text",
                Type?.ToString()
            );
        }
    }
}