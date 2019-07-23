using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Policies;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;


namespace Disunity.Store.Pages.Users {

    [Breadcrumb]
    public class Details : PageModel {

        private readonly ILogger<Details> _logger;
        private readonly ApplicationDbContext _context;

        [FromRoute] public string UserSlug { get; set; }

        public Org Org { get; set; }

        public Details(ILogger<Details> logger, ApplicationDbContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync() {
            Org = await _context.Orgs
                                .Include(o => o.Mods)
                                .ThenInclude(m => m.Latest)
                                .ThenInclude(v => v.VersionNumber)
                                .FirstOrDefaultAsync(o => o.Slug == UserSlug);

            ViewData["BreadcrumbNode"] = new RazorPageBreadcrumbNode("/Users/Details", Org.DisplayName) {
                RouteValues = new {UserSlug}
            };

        }

    }

}