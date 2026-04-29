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

        public List<SelectListItem> TypeItems { get; set; } = new List<SelectListItem>();

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var item = await _context.Specializations.FindAsync(id);
            if (item == null)
                return NotFound();

            Specialization = item;

            TypeItems = Enum.GetValues(typeof(FieldType))
                .Cast<FieldType>()
                .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TypeItems = Enum.GetValues(typeof(FieldType))
                    .Cast<FieldType>()
                    .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
                    .ToList();
                return Page();
            }

            var existing = await _context.Specializations.FindAsync(Specialization.SpecializationId);
            if (existing == null)
                return NotFound();

            existing.Name = Specialization.Name;
            existing.Description = Specialization.Description;
            existing.Type = Specialization.Type;

            _context.Specializations.Update(existing);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}