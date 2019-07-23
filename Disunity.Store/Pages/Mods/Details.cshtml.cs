using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;


namespace Disunity.Store.Pages.Mods {

    public class DependencyList {

        public IEnumerable<ModDependency> Dependencies { get; set; }

        public bool ShowDependent { get; set; } = true;

        public bool ShowDependency { get; set; } = true;

    }

    [Breadcrumb(FromPage = typeof(Index))]
    public class Details : PageModel {

        private readonly ILogger<Details> _logger;
        private readonly ApplicationDbContext _context;

        [FromRoute] public string OwnerSlug { get; set; }
        [FromRoute] public string ModSlug { get; set; }
        [FromRoute] public string VersionNumber { get; set; }

        public ModVersion ModVersion { get; set; }

        public DependencyList Dependencies { get; set; }
        public DependencyList Dependents { get; set; }

        public string CreateAtString { get; set; }

        public Details(ILogger<Details> logger, ApplicationDbContext context) {
            _logger = logger;
            _context = context;

        }

        public async Task<IActionResult> OnGetAsync() {
            IQueryable<ModVersion> query = _context.ModVersions
                                                   .Include(v => v.Mod).ThenInclude(m => m.Owner)
                                                   .Include(v => v.VersionNumber)
                                                   .Include(v => v.ModDependencies)
                                                   .ThenInclude(d => d.Dependency).ThenInclude(m => m.Owner)
                                                   .Include(v => v.ModDependencies)
                                                   .ThenInclude(d => d.Dependency).ThenInclude(m => m.Latest)
                                                   .Include(v => v.ModDependencies)
                                                   .ThenInclude(d => d.MinVersion).ThenInclude(v => v.VersionNumber)
                                                   .Include(v => v.ModDependencies)
                                                   .ThenInclude(d => d.MaxVersion).ThenInclude(v => v.VersionNumber)
                                                   .Where(v => v.Mod.Owner.Slug == OwnerSlug && v.Mod.Slug == ModSlug)
                                                   .OrderByVersionDescending();

            if (!string.IsNullOrEmpty(VersionNumber)) {
                query = query.FindExactVersion(VersionNumber);
            }

            ModVersion = await query.FirstOrDefaultAsync();

            if (ModVersion == null) {
                _logger.LogInformation($"No modversion found for {OwnerSlug}/{ModSlug}@{VersionNumber}");
                return NotFound();
            }

            CreateAtString = ModVersion.CreatedAt.ToString("o");

            Dependents = new DependencyList() {
                Dependencies = await _context.ModDependencies
                                             .Include(d => d.Dependent)
                                             .ThenInclude(v => v.Mod)
                                             .ThenInclude(m => m.Owner)
                                             .Include(d => d.Dependent)
                                             .ThenInclude(v => v.VersionNumber)
                                             .Include(d => d.Dependency)
                                             .Where(d => d.DependencyId == ModVersion.Id)
                                             .ToListAsync(),
                ShowDependent = true,
                ShowDependency = false
            };

            Dependencies = new DependencyList() {
                Dependencies = ModVersion.Dependencies,
                ShowDependent = false
            };

            ViewData["ModVersions"] = await _context.ModVersions
                                                    .Where(v => v.ModId == ModVersion.ModId)
                                                    .Select(v => new {
                                                        VersionNumber = v.VersionNumber.ToString(), v.Id
                                                    }).ToListAsync();


            var ownerNode = new RazorPageBreadcrumbNode("/Users/Details", ModVersion.Mod.Owner.DisplayName) {
                OverwriteTitleOnExactMatch = true,
                RouteValues = new {userSlug = ModVersion.Mod.Owner.Slug}
            };

            var modNode = new RazorPageBreadcrumbNode("/Mods/Details", ModVersion.DisplayName) {
                OverwriteTitleOnExactMatch = true,
                Parent = ownerNode
            };

            ViewData["BreadcrumbNode"] = modNode;
            ViewData["Title"] = ModVersion.DisplayName;

            return Page();
        }

    }

}