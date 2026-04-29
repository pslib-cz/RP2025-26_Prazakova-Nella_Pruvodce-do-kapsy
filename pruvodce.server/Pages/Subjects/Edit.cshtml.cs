using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Subjects
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subject Subject { get; set; } = default!;

        [BindProperty]
        public List<string> SelectedTeacherIds { get; set; } = new();

        [BindProperty]
        public List<string> SelectedPointIds { get; set; } = new();

        public MultiSelectList TeacherItems { get; set; } = default!;
        public MultiSelectList PointItems { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var item = await _context.Subjects
                .Include(s => s.Points)
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (item == null)
            {
                return NotFound();
            }

            Subject = item;

            SelectedPointIds = Subject.Points
                .Select(p => p.PointId)
                .ToList();

            await LoadSelectListsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Subject.Teachers");
            ModelState.Remove("Subject.Points");

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var subjectToUpdate = await _context.Subjects
                .Include(s => s.Points)
                .FirstOrDefaultAsync(s => s.SubjectId == Subject.SubjectId);

            if (subjectToUpdate == null)
            {
                return NotFound();
            }

            subjectToUpdate.Name = Subject.Name;
            subjectToUpdate.Acronym = Subject.Acronym;
            subjectToUpdate.Note = Subject.Note;

            subjectToUpdate.Points.Clear();

            var selectedPoints = await _context.Points
                .Where(p => SelectedPointIds.Contains(p.PointId))
                .ToListAsync();

            foreach (var point in selectedPoints)
            {
                subjectToUpdate.Points.Add(point);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadSelectListsAsync()
        {
            var teachers = await _context.Teachers
                .OrderBy(t => t.LastN)
                .ThenBy(t => t.FirstN)
                .ToListAsync();

            var points = await _context.Points
                .OrderBy(p => p.Label)
                .ToListAsync();

            PointItems = new MultiSelectList(
                points,
                "PointId",
                "Label",
                SelectedPointIds
            );
        }
    }
}