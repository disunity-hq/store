using System.IO;
using System.Threading.Tasks;
using Disunity.Store.Areas.Mods.Pages;
using Disunity.Store.Shared.Archive;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Disunity.Store.Areas.API.v1.Mods {

    [ApiController]
    [Route("api/v{version:apiVersion}/mods/upload")]
    public class UploadController : ControllerBase {
        
        [FromForm] 
        public IFormFile ArchiveUpload { get; set; }
        
        private readonly ILogger<Upload> _logger;
        private readonly ArchiveValidator _validator;
        
        public UploadController(ILogger<Upload> logger, ArchiveValidator validator)
        {
            _logger = logger;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Post() {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var archive = await _validator.ValidateAsync(ArchiveUpload, ModelState, "application/zip");
            
//            if (ModelState.ErrorCount == 0) {
//                _logger.LogError($"We got an archive: {archive.Manifest.Name}");
//            } else {
//                foreach (var (key, value) in ModelState) {
//                    foreach (var error in value.Errors) {
//                        _logger.LogError($"{key}: {error.ErrorMessage}");
//                    }
//                }
//            }
//
//            var tmpPath = Path.GetTempPath();
//            var filePath = Path.Join(tmpPath, ArchiveUpload.FileName);
//
//            if (ArchiveUpload.Length > 0) {
//                using (var stream = new FileStream(filePath, FileMode.Create)) {
//                    await ArchiveUpload.CopyToAsync(stream);
//                }
//            }

            return new JsonResult(new { Status = "yay" });
        }

    }

}