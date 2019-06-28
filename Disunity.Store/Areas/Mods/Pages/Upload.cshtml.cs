using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Disunity.Store.Shared.Archive;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Overby.Extensions.AsyncBinaryReaderWriter;

namespace Disunity.Store.Areas.Mods.Pages {

    public class Upload : PageModel {

        [BindProperty] public IFormFile ArchiveUpload { get; set; }
        
        private readonly ILogger<Upload> _logger;
        private readonly ArchiveValidator _validator;
        
        public Upload(ILogger<Upload> logger, ArchiveValidator validator)
        {
            _logger = logger;
            _validator = validator;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _validator.Validate(ArchiveUpload, ModelState);

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