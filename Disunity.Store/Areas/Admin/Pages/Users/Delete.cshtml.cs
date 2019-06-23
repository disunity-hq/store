using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly Disunity.Store.Data.ApplicationDbContext _context;

        public DeleteModel(Disunity.Store.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserIdentity = await _context.Users.FindAsync(id);

            if (UserIdentity != null)
            {
                _context.Users.Remove(UserIdentity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
