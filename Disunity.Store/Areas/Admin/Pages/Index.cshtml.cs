using Disunity.Store.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Areas.Admin.Pages {

    [Breadcrumb("Admin", FromPage = typeof(IndexModel))]
    public class AdminIndexModel: PageModel {

        public void OnGet() { }

    }

}