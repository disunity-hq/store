using System.Threading.Tasks;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Disunity.Store.Areas.Admin.Pages.Mods {

    public class CreateModel : PageModel {

        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public Mod Mod { get; set; }

        public IActionResult OnGet() {
            ViewData["LatestId"] = new SelectList(_context.ModVersions, "ID", "Description");
            ViewData["OwnerId"] = new SelectList(_context.Orgs, "ID", "ID");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Mods.Add(Mod);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }

}