using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Teachers
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var item = await _context.Teachers.FindAsync(id);
            if (item == null)
                return NotFound();

            Teacher = item;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existing = await _context.Teachers.FindAsync(Teacher.TeacherId);
            if (existing == null)
                return NotFound();

            existing.FirstN = Teacher.FirstN;
            existing.LastN = Teacher.LastN;
            existing.Degree = Teacher.Degree;
            existing.Note = Teacher.Note;

            _context.Teachers.Update(existing);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}