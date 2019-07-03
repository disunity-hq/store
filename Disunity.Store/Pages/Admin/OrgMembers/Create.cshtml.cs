using System.Threading.Tasks;

using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Disunity.Store.Pages.Admin.OrgMembers {

    public class CreateModel : PageModel {

        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public OrgMember OrgMember { get; set; }

        public IActionResult OnGet() {
            ViewData["OrgId"] = new SelectList(_context.Orgs, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.OrgMembers.Add(OrgMember);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }

}