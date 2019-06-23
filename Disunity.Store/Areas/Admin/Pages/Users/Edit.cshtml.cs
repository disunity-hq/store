using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly Disunity.Store.Data.ApplicationDbContext _context;

        public EditModel(Disunity.Store.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserIdentity UserIdentity { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserIdentity = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (UserIdentity == null)
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

            _context.Attach(UserIdentity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserIdentityExists(UserIdentity.Id))
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

        private bool UserIdentityExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
