using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Subjects
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subject Subject { get; set; } = default!;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrEmpty(Subject.SubjectId))
                Subject.SubjectId = Guid.NewGuid().ToString();

            _context.Subjects.Add(Subject);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}