using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using pruvodce.server.Data;
using pruvodce.server.Models;

namespace pruvodce.server.Pages.Specializations
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Specialization Specialization { get; set; } = new Specialization
        {
            SpecializationId = string.Empty,
            Name = string.Empty,
            Description = string.Empty
        };

        public List<SelectListItem> TypeItems { get; set; } = new();

        public void OnGet()
        {
            LoadTypeItems();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Specialization.SpecializationId))
            {
                Specialization.SpecializationId = Guid.NewGuid().ToString();
            }

            // use nameof to avoid magic strings
            ModelState.Remove($"Specialization.{nameof(Specialization.SpecializationId)}");

            if (!ModelState.IsValid)
            {
                // repopulate all selects before returning the page
                LoadTypeItems();
                return Page();
            }

            _context.Specializations.Add(Specialization);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private void LoadTypeItems()
        {
            TypeItems = Enum.GetValues(typeof(FieldType))
                .Cast<FieldType>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();
        }
    }
}