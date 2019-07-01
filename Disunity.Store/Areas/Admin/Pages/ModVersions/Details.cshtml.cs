using System.Threading.Tasks;

using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Areas.Admin.Pages.ModVersions {

    public class DetailsModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context) {
            _context = context;
        }

        public ModVersion ModVersion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            ModVersion = await _context.ModVersions.FirstOrDefaultAsync(m => m.Id == id);

            if (ModVersion == null) {
                return NotFound();
            }

            return Page();
        }

    }

}