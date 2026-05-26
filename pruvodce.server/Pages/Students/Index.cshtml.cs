using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudentNote StudentNote { get; set; } = new();

        public List<SelectListItem> TeacherItems { get; set; } = [];
        public List<SelectListItem> SubjectItems { get; set; } = [];
        public List<SelectListItem> PointItems { get; set; } = [];

        public void OnGet()
        {
            LoadSelects();
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            LoadSelects();

            StudentNote.TargetType = StudentNote.TargetType.ToUpper();

            if (!ModelState.IsValid)
                return Page();

            switch (StudentNote.TargetType)
            {
                case "TEACHER":
                    if (string.IsNullOrEmpty(StudentNote.TeacherId))
                    {
                        ModelState.AddModelError("", "Vyberte učitele.");
                        return Page();
                    }
                    break;

                case "SUBJECT":
                    if (string.IsNullOrEmpty(StudentNote.SubjectId))
                    {
                        ModelState.AddModelError("", "Vyberte předmět.");
                        return Page();
                    }
                    break;
            }

            _context.StudentNotes.Add(StudentNote);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Poznámka byla přidána.";

            return RedirectToPage();
        }

        private void LoadSelects()
        {
            TeacherItems = _context.Teachers
                .Select(t => new SelectListItem
                {
                    Value = t.TeacherId,
                    Text = t.FirstN + " " + t.LastN
                })
                .ToList();

            SubjectItems = _context.Subjects
                .Select(s => new SelectListItem
                {
                    Value = s.SubjectId,
                    Text = s.Name
                })
                .ToList();
        }
    }
}