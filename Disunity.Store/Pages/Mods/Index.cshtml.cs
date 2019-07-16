using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Pages.Mods {

    [Breadcrumb("Mods")]
    public class Index : PageModel {

        private readonly ApplicationDbContext _context;

        public Index(ApplicationDbContext context) {
            _context = context;

        }

        public List<Mod> Mods { get; set; }

        [BindProperty(SupportsGet = true)] public string Title { get; set; }
        [BindProperty(SupportsGet = true)] public string Target { get; set; }


        public async Task OnGetAsync() {


            IQueryable<Mod> mods = _context.Mods
                                           .Include(m => m.Latest)
                                           .ThenInclude(v => v.VersionNumber);

            if (!string.IsNullOrWhiteSpace(Title)) {
                mods = mods.Where(m => EF.Functions.ILike(m.Latest.DisplayName, $"%{Title}%"));
            }

            if (!string.IsNullOrWhiteSpace(Target)) {
                mods = mods.Where(m => m.Versions.Any(v => v.TargetCompatibilities.Any(c => c.Target.Slug == Target)));
            }


            Mods = await mods.ToListAsync();
        }

    }

}