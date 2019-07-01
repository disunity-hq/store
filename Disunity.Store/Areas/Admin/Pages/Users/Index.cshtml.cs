using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Areas.Admin.Pages.Users {

    public class IndexModel : PageModel {

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context) {
            _context = context;
        }

        public IList<UserIdentity> UserIdentity { get; set; }

        public async Task OnGetAsync() {
            UserIdentity = await _context.Users.ToListAsync();
        }

    }

}