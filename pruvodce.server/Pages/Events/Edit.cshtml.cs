using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Events
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Event Event { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.Events.FindAsync(id.Value);
            if (entity == null)
                return NotFound();

            Event = entity;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existing = await _context.Events.FindAsync(Event.EventId);
            if (existing == null)
                return NotFound();

            existing.Name = Event.Name;
            existing.StartDate = Event.StartDate;
            existing.EndDate = Event.EndDate;
            existing.IsActive = Event.IsActive;
            existing.Description = Event.Description;

            _context.Events.Update(existing);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}