using System.Threading.Tasks;

using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Pages.Admin.Mods {

    public class DetailsModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context) {
            _context = context;
        }

        public Mod Mod { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Mod = await _context.Mods
                                .Include(m => m.Latest)
                                .Include(m => m.Owner).FirstOrDefaultAsync(m => m.Id == id);

            if (Mod == null) {
                return NotFound();
            }

            return Page();
        }

    }

}