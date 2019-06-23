using System.Threading.Tasks;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.Admin.Pages.Users {

    public class DeleteModel : PageModel {

        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public UserIdentity UserIdentity { get; set; }

        public async Task<IActionResult> OnGetAsync(string id) {
            if (id == null) {
                return NotFound();
            }

            UserIdentity = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (UserIdentity == null) {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id) {
            if (id == null) {
                return NotFound();
            }

            UserIdentity = await _context.Users.FindAsync(id);

            if (UserIdentity != null) {
                _context.Users.Remove(UserIdentity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }

}