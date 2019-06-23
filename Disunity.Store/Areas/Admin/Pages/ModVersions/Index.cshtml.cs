using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.ModVersions
{
    public class IndexModel : PageModel
    {
        private readonly Disunity.Store.Data.ApplicationDbContext _context;

        public IndexModel(Disunity.Store.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ModVersion> ModVersion { get;set; }

        public async Task OnGetAsync()
        {
            ModVersion = await _context.ModVersions.ToListAsync();
        }
    }
}
