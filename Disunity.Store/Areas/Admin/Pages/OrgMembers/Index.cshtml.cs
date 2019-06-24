using System.Collections.Generic;
using System.Threading.Tasks;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.Admin.Pages.OrgMembers {

    public class IndexModel : PageModel {

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context) {
            _context = context;
        }

        public IList<OrgMember> OrgMember { get; set; }

        public async Task OnGetAsync() {
            OrgMember = await _context.OrgMembers
                                      .Include(o => o.Org)
                                      .Include(o => o.User).ToListAsync();
        }

    }

}