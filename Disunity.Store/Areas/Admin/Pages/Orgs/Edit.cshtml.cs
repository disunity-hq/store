using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.Orgs
{
    public class EditModel : PageModel
    {
        private readonly Disunity.Store.Data.ApplicationDbContext _context;

        public EditModel(Disunity.Store.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Org Org { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Org = await _context.Orgs.FirstOrDefaultAsync(m => m.ID == id);

            if (Org == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Org).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrgExists(Org.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrgExists(int id)
        {
            return _context.Orgs.Any(e => e.ID == id);
        }
    }
}
