using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;

namespace Disunity.Store.Areas_Admin_Pages_OrgMembers
{
    public class DetailsModel : PageModel
    {
        private readonly Disunity.Store.Shared.Data.ApplicationDbContext _context;

        public DetailsModel(Disunity.Store.Shared.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
