using System;
using System.Linq;

using Disunity.Store.Pages.Mods;
using Disunity.Store.Shared.Archive;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Schema;


namespace Disunity.Store.Areas.API.v1.Mods {

    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/mods/upload")]
    public class UploadController : ControllerBase {

        private readonly Func<IFormFile, Archive> _archiveFactory;

        private readonly ILogger<Upload> _logger;

        public UploadController(ILogger<Upload> logger,
                                Func<IFormFile, Archive> archiveFactory) {
            _logger = logger;
            _archiveFactory = archiveFactory;
        }

        [FromForm] public IFormFile ArchiveUpload { get; set; }

        protected object FormatSchemaError(ValidationError e) {
            return new {
                Kind = e.ErrorType,
                e.Path,
                e.Message,
                e.LineNumber,
                e.LinePosition
            };
        }

        [HttpGet]
        public IActionResult Get() {
            return new JsonResult("Hello World!");
        }

        [HttpPost]
        public IActionResult Post() {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            try {
                var archive = _archiveFactory(ArchiveUpload);
                return new JsonResult(new {archive.Manifest.DisplayName});
            }
            catch (ManifestSchemaException e) {
                return BadRequest(new {
                    Type = "ManifestSchemaException",
                    Errors = e.Errors.Select(FormatSchemaError)
                });
            }
        }

    }

}