using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.TagHelpers {

    public class PageScriptTagHelper : TagHelper {
        
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<PageScriptTagHelper> _logger;

        public PageScriptTagHelper(IHostingEnvironment env, IHttpContextAccessor context, ILogger<PageScriptTagHelper> logger) {
            _env = env;
            _context = context;
            _logger = logger;
        }        
        
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            string path = (await output.GetChildContentAsync()).GetContent().ToLower();
            string addendum = string.IsNullOrEmpty(path) ? "\n<script>InitPageScript();</script>" : "";

            if (string.IsNullOrEmpty(path)) {
                string page = _context.HttpContext.GetRouteData().Values["page"] as string;
                path = page.Substring(1, page.Length - 1).ToLower();
            }
            
            string src;
            
            if (_env.IsDevelopment()) {
                src = $"/js/{path}.js";
            } else {
                src = $"/js/{path}.min.js";
            }
            
            string content = $"<script src=\"{src}\"></script>{addendum}";

            output.TagName = null;
            output.Content.SetHtmlContent(content);
        }

    }

}