using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.Mods
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
        ViewData["LatestId"] = new SelectList(_context.ModVersions, "ID", "Description");
        ViewData["OwnerId"] = new SelectList(_context.Orgs, "ID", "ID");
            return Page();
        }

        [BindProperty]
        public Mod Mod { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Mods.Add(Mod);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}