using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Areas.Mods.Pages {

    public class Upload : PageModel {

        private readonly ILogger<Upload> _logger;

        public Upload(ILogger<Upload> logger) {
            _logger = logger;
        }

        [BindProperty] public IFormFile ArchiveUpload { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            var tmpPath = Path.GetTempPath();
            var filePath = Path.Join(tmpPath, ArchiveUpload.FileName);

            if (ArchiveUpload.Length > 0) {
                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    await ArchiveUpload.CopyToAsync(stream);
                }
            }


            return new OkResult();
        }

    }

}