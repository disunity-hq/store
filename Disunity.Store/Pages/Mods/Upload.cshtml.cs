using System.IO;
using System.Threading.Tasks;
using Disunity.Store.Shared.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using SmartBreadcrumbs.Attributes;


namespace Disunity.Store.Pages.Mods
{

    [Breadcrumb("Mod upload", FromPage = typeof(IndexModel))]
    public class Upload : PageModel
    {

        private readonly ILogger<Upload> _logger;
        private readonly IB2Service _b2Service;

        public Upload(ILogger<Upload> logger, IB2Service b2Service)
        {
            _logger = logger;
            _b2Service = b2Service;
        }

        [BindProperty] public IFormFile ArchiveUpload { get; set; }

        public void OnGet()
        {
            _logger.LogInformation("Upload page get");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var tmpPath = Path.GetTempPath();
            var filePath = Path.Join(tmpPath, ArchiveUpload.FileName);

            _logger.LogInformation($"Recieving file upload {ArchiveUpload.FileName}");

            if (ArchiveUpload.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                using (var memoryStream = new MemoryStream())
                {
                    await ArchiveUpload.CopyToAsync(stream);
                    await ArchiveUpload.CopyToAsync(memoryStream);

                    await _b2Service.UploadFile(memoryStream.ToArray(), ArchiveUpload.FileName);
                }
            }

            return new OkResult();
        }

    }

}