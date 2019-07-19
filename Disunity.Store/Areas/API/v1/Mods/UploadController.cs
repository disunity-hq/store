using System;
using System.Linq;

using Disunity.Store.Pages.Mods;
using Disunity.Store.Shared.Archive;
using Disunity.Store.Shared.Backblaze;

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

        private readonly IB2Service _b2;

        public UploadController(ILogger<Upload> logger,
                                Func<IFormFile, Archive> archiveFactory,
                                IB2Service b2) {
            _logger = logger;
            _archiveFactory = archiveFactory;
            _b2 = b2;
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

                if (_b2.ServiceConfigured) {
                    _logger.LogInformation($"Uploading {archive.Manifest.ModID}.zip");

                    using (var uploadStream = _b2.GetUploadStream($"{archive.Manifest.ModID}.zip")) {
                        ArchiveUpload.CopyTo(uploadStream);
                        uploadStream.FinalizeUpload();
                    }
                }

                return new JsonResult(new {archive.Manifest.DisplayName});
            }
            catch (Exception e) {
                var Type = e.GetType().Name;
                var Errors = new object[] { };

                if (e is ManifestSchemaException schemaExc) {
                    Errors = schemaExc.Errors.Select(FormatSchemaError).ToArray();
                } else if (e is ArchiveFormFileValidationError formFileError) {
                    Errors = new[] {formFileError.Message};
                } else if (e is ArchiveLoadException archiveExc) {
                    Errors = new[] {archiveExc.Message};
                } else {
                    _logger.LogError(e, "");
                }

                return BadRequest(new {Type, Errors});
            }
        }

    }

}