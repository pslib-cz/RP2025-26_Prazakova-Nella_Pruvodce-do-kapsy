using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Specializations
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Specialization Specialization { get; set; } = default!;

        public List<SelectListItem> TypeItems { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var item = await _context.Specializations
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SpecializationId == id);

            if (item == null)
            {
                return NotFound();
            }

            Specialization = item;

            LoadSelectLists();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Specialization.Points");

            if (!ModelState.IsValid)
            {
                LoadSelectLists();
                return Page();
            }

            var existing = await _context.Specializations
                .FirstOrDefaultAsync(s => s.SpecializationId == Specialization.SpecializationId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = Specialization.Name;
            existing.Description = Specialization.Description;
            existing.Type = Specialization.Type;

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private void LoadSelectLists()
        {
            TypeItems = Enum.GetValues(typeof(FieldType))
                .Cast<FieldType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                })
                .ToList();
        }
    }
}