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

        public PageScriptTagHelper(IHostingEnvironment env, IHttpContextAccessor context,
                                   ILogger<PageScriptTagHelper> logger) {
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

        private string GetAddendum(string route) {
            var template = $@"\n<script>

            try {{
                InitPageScript();
            }}
            catch (error) {{
                console.error('Page script failed to initialize for {route}\n' + error);
            }}

            </script>";

            return template;
        }

        private string GetContent(string path) {
            var src = GetSrcForPath(path);
            return $"<script src=\"{src}\"></script>";
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            var path = await GetContentForTag(output);

            string content;

            if (string.IsNullOrEmpty(path)) {
                var route = GetPathForPage();
                var addendum = GetAddendum(route);
                content = GetContent(route) + addendum;
            } else {
                content = GetContent(path);
            }

            output.TagName = null;
            output.Content.SetHtmlContent(content);
        }

    }

}