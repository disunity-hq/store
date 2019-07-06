using System.Linq;
using System.Threading.Tasks;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.Admin.Pages.Orgs
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty] public Org Org { get; set; }

        [BindProperty(SupportsGet = true, Name = "org")]
        public string OrgName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (OrgName == null)
            {
                return NotFound();
            }

            Org = await _context.Orgs.FirstOrDefaultAsync(m => m.Name == OrgName);

            if (Org == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Org).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrgExists(Org.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrgExists(int id)
        {
            return _context.Orgs.Any(e => e.Id == id);
        }
    }
}