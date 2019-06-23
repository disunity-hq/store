using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Data;

namespace Disunity.Store.Areas.Admin.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly Disunity.Store.Data.ApplicationDbContext _context;

        public IndexModel(Disunity.Store.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<UserIdentity> UserIdentity { get;set; }

        public async Task OnGetAsync()
        {
            UserIdentity = await _context.Users.ToListAsync();
        }
    }
}
