using Microsoft.AspNetCore.Mvc.RazorPages;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages {

    [Breadcrumb("Admin", FromPage = typeof(IndexModel))]
    public class AdminIndexModel : PageModel {

        public void OnGet() { }

    }

}