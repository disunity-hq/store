using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.Users
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
        public UserIdentity UserIdentity { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Users.Add(UserIdentity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}