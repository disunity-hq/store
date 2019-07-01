using System.Threading.Tasks;

using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Areas.Admin.Pages.OrgMembers {

    public class DetailsModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context) {
            _context = context;
        }

        public OrgMember OrgMember { get; set; }

        public async Task<IActionResult> OnGetAsync(string orgId, string userId) {
            if (orgId == null || userId == null) {
                return NotFound();
            }

            OrgMember = await _context.OrgMembers
                                      .Include(o => o.Org)
                                      .Include(o => o.User)
                                      .FirstOrDefaultAsync(m => m.Org.Name == orgId && m.User.UserName == userId);

            if (OrgMember == null) {
                return NotFound();
            }

            return Page();
        }

    }

}