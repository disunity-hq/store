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

        private string GetSrcForPath(string path) {
            if (_env.IsDevelopment()) {
                return $"/js/{path}.js";
            } else {
                return $"/js/{path}.min.js";
            }
        }

        private async Task<string> GetContentForTag(TagHelperOutput output) {
            return (await output.GetChildContentAsync()).GetContent().ToLower();
        }

        private string GetPathForPage() {
            var page = _context.HttpContext.GetRouteData().Values["page"] as string;
            return page?.Substring(1, page.Length - 1).ToLower();
        }

        private string GetAddendum(string path) {
            return string.IsNullOrEmpty(path) ? "\n<script>InitPageScript();</script>" : "";
        }

        private string GetContent(string src, string addendum) {
            return $"<script src=\"{src}\"></script>{addendum}";
        }
        
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            var path = await GetContentForTag(output);
            var addendum = GetAddendum(path); 

            if (string.IsNullOrEmpty(path)) {
                path = GetPathForPage();
            }

            var src = GetSrcForPath(path);
            var content = GetContent(src, addendum); 

            output.TagName = null;
            output.Content.SetHtmlContent(content);
        }

    }

}