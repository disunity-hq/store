using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;

namespace Disunity.Store.Areas_Admin_Pages_OrgMembers
{
    public class CreateModel : PageModel
    {
        private readonly Disunity.Store.Shared.Data.ApplicationDbContext _context;

        public CreateModel(Disunity.Store.Shared.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["OrgId"] = new SelectList(_context.Orgs, "Id", "Id");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public OrgMember OrgMember { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.OrgMembers.Add(OrgMember);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}