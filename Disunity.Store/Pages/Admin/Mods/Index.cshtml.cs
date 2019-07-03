using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Admin.Mods {

    [Breadcrumb("Mods", FromPage = typeof(Disunity.Store.Pages.Admin.IndexModel))]
    public class IndexModel : PageModel {

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context) {
            _context = context;
        }

        public IList<Mod> Mod { get; set; }

        public async Task OnGetAsync() {
            Mod = await _context.Mods
                                .Include(m => m.Latest)
                                .Include(m => m.Owner).ToListAsync();
        }

    }

}