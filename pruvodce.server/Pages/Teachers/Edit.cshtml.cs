using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;
using System.Text.Json;
 
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
 
        [BindProperty]
        public List<string> SelectedNoteIds { get; set; } = new();
 
        public List<StudentNote> AvailableNotes { get; set; } = new();
 
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();
 
            var item = await _context.Teachers
                .Include(t => t.Notes)
                .FirstOrDefaultAsync(t => t.TeacherId == id);
 
            if (item == null)
                return NotFound();
 
            Teacher = item;
 
            if (!string.IsNullOrEmpty(Teacher.SelectedNoteIds))
            {
                try
                {
                    SelectedNoteIds = JsonSerializer.Deserialize<List<string>>(Teacher.SelectedNoteIds) ?? new();
                }
                catch
                {
                    SelectedNoteIds = new();
                }
            }
 
            await LoadAvailableNotesAsync();
 
            return Page();
        }
 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAvailableNotesAsync();
                return Page();
            }
 
            var existing = await _context.Teachers
                .FirstOrDefaultAsync(t => t.TeacherId == Teacher.TeacherId);
 
            if (existing == null)
                return NotFound();
 
            existing.FirstN = Teacher.FirstN;
            existing.LastN = Teacher.LastN;
            existing.Degree = Teacher.Degree;
 
            var limitedNoteIds = SelectedNoteIds.Take(3).ToList();
            existing.SelectedNoteIds = limitedNoteIds.Any()
                ? JsonSerializer.Serialize(limitedNoteIds)
                : null;
 
            await _context.SaveChangesAsync();
 
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostDeleteNoteAsync(string noteId)
        {
            var note = await _context.StudentNotes
                .FirstOrDefaultAsync(n => n.StudentNoteId == noteId);

            if (note != null)
            {
                _context.StudentNotes.Remove(note);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
 
        private async Task LoadAvailableNotesAsync()
        {
            AvailableNotes = await _context.StudentNotes
                .Where(n => n.TargetType.ToUpper() == "TEACHER" && n.TeacherId == Teacher.TeacherId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}