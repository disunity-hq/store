using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.Mods
{
    public class DeleteModel : PageModel
    {
        private readonly Disunity.Store.Data.ApplicationDbContext _context;

        public DeleteModel(Disunity.Store.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mod Mod { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Mod = await _context.Mods
                .Include(m => m.Latest)
                .Include(m => m.Owner).FirstOrDefaultAsync(m => m.ID == id);

            if (Mod == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Mod = await _context.Mods.FindAsync(id);

            if (Mod != null)
            {
                _context.Mods.Remove(Mod);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
