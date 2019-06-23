using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.Orgs
{
    public class IndexModel : PageModel
    {
        private readonly Disunity.Store.Data.ApplicationDbContext _context;

        public IndexModel(Disunity.Store.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Org> Org { get;set; }

        public async Task OnGetAsync()
        {
            Org = await _context.Orgs.ToListAsync();
        }
    }
}
