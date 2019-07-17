using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Data;
using Disunity.Store.Shared.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SmartBreadcrumbs.Attributes;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Pages.Mods
{

    [Breadcrumb("Mods")]
    public class Index : PageModel, IOrderableModel
    {

        private readonly ApplicationDbContext _context;

        public Index(ApplicationDbContext context)
        {
            _context = context;

        }

        public List<Mod> Mods { get; set; }

        [BindProperty(SupportsGet = true)] public string Title { get; set; }
        [BindProperty(SupportsGet = true)] public string Target { get; set; }

        [BindProperty(SupportsGet = true)] public string OrderBy { get; set; } = "Latest";

        [BindProperty(SupportsGet = true)] public Ordering? Order { get; set; }

        public IEnumerable<string> OrderByChoices => OrderOptions.Keys;

        public Dictionary<string, Expression<Func<Mod, IComparable>>> OrderOptions { get; set; } = new Dictionary<string, Expression<Func<Mod, IComparable>>>() {
            { "Latest", m => m.Latest.CreatedAt },
            { "Downloads", m => m.Versions.Sum(v => v.Downloads)},
            { "Name",  m => m.DisplayName},
        };

        public async Task OnGetAsync()
        {
            IQueryable<Mod> mods = _context.Mods
                    .Include(m => m.Latest)
                    .ThenInclude(v => v.VersionNumber);

            if (!string.IsNullOrWhiteSpace(Title))
            {
                mods = mods.Where(m => EF.Functions.ILike(m.Latest.DisplayName, $"%{Title}%"));
            }

            if (!string.IsNullOrWhiteSpace(Target))
            {
                mods = mods.Where(m => m.Versions.Any(v => v.TargetCompatibilities.Any(c => c.Target.Slug == Target)));
            }

            if (!string.IsNullOrWhiteSpace(OrderBy))
            {

                var ordering = OrderOptions.GetValueOrDefault(OrderBy);
                if (ordering != null)
                {
                    if (Order != null && Order == Ordering.Desc)
                    {
                        mods = mods.OrderByDescending(ordering);
                    }
                    else
                    {
                        mods = mods.OrderBy(ordering);
                    }

                }
            }


            Mods = await mods.ToListAsync();
        }

    }

}