using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.ModVersions
{
    public class CreateModel : PageModel
    {
        private readonly Disunity.Store.Data.ApplicationDbContext _context;

        public CreateModel(Disunity.Store.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ModVersion ModVersion { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ModVersions.Add(ModVersion);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}