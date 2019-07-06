using System.Collections.Generic;
using System.Threading.Tasks;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.Admin.Pages.Orgs {

    public class IndexModel {

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context) {
            _context = context;
        }

        public IList<Org> Org { get; set; }

        public async Task OnGetAsync() {
            Org = await _context.Orgs.ToListAsync();
        }

    }

}