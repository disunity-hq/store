using System.Threading.Tasks;

using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Disunity.Store.Pages.Admin.Users {

    public class CreateModel : PageModel {

        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public UserIdentity UserIdentity { get; set; }

        public IActionResult OnGet() {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Users.Add(UserIdentity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }

}