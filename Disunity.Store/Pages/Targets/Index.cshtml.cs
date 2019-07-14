using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Targets {

    [Breadcrumb("Targets")]
    public class Index : PageModel {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<Index> _logger;

        public IList<Target> Targets { get; set; }

        public Index(ApplicationDbContext context, ILogger<Index> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task OnGet() {
            Targets = await _context.Targets.Include(t => t.Latest).ToListAsync();

            _logger.LogInformation(
                $"Found Targets {Targets.Count} with {Targets.Count(t => t.Latest != null)} Latest Versions");

            _logger.LogInformation($"Icon URL: {Targets.FirstOrDefault()?.Latest.IconUrl}");
        }

    }

}