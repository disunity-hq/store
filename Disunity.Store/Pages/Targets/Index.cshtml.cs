using Disunity.Store.Entities;
using Disunity.Store.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Disunity.Store.Pages.Targets
{

    [Breadcrumb("Targets")]
    public class Index : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<Index> _logger;

        public IList<Target> Targets { get; set; }

        [BindProperty(SupportsGet = true)] public string Title { get; set; }
        [BindProperty(SupportsGet = true)] public string OrderBy { get; set; } = "Name";

        public Dictionary<string, Expression<Func<Target, IComparable>>> OrderOptions = new Dictionary<string, Expression<Func<Target, IComparable>>>() {
            {"Name",  t=>t.DisplayName
    },
            {"Total Mods",  t=>t.CompatibleMods.Count()
},
        };

        public Index(ApplicationDbContext context, ILogger<Index> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            IQueryable<Target> targets = _context.Targets
                                                 .Include(t => t.Latest)
                                                 .Include(t => t.Compatibilities);

            if (!string.IsNullOrWhiteSpace(Title))
            {
                targets = targets.Where(t => EF.Functions.ILike(t.Latest.DisplayName, $"%{Title}%"));
            }

            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                var ordering = OrderOptions.GetValueOrDefault(OrderBy);
                if (ordering != null)
                {
                    targets = targets.OrderBy(ordering);
                }
            }

            Targets = await targets.ToListAsync();
        }

    }

}