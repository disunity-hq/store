using System.Collections.Generic;
using System.Threading.Tasks;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Shared.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.Admin.Pages.ModVersions {

    public class IndexModel : PageModel {

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context) {
            _context = context;
        }

        public IList<ModVersion> ModVersion { get; set; }

        public async Task OnGetAsync() {
            ModVersion = await _context.ModVersions.ToListAsync();
        }

    }

}