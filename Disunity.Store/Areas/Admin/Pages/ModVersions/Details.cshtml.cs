using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.ModVersions
{
    public class DetailsModel : PageModel
    {
        private readonly Disunity.Store.Data.ApplicationDbContext _context;

        public DetailsModel(Disunity.Store.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ModVersion ModVersion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ModVersion = await _context.ModVersions.FirstOrDefaultAsync(m => m.ID == id);

            if (ModVersion == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
