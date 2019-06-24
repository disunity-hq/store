using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;

namespace Disunity.Store.Areas_Admin_Pages_OrgMembers
{
    public class EditModel : PageModel
    {
        private readonly Disunity.Store.Shared.Data.ApplicationDbContext _context;

        public EditModel(Disunity.Store.Shared.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrgMember OrgMember { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrgMember = await _context.OrgMembers
                .Include(o => o.Org)
                .Include(o => o.User).FirstOrDefaultAsync(m => m.UserId == id);

            if (OrgMember == null)
            {
                return NotFound();
            }
           ViewData["OrgId"] = new SelectList(_context.Orgs, "Id", "Id");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(OrgMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrgMemberExists(OrgMember.UserId))
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

        private bool OrgMemberExists(string id)
        {
            return _context.OrgMembers.Any(e => e.UserId == id);
        }
    }
}
