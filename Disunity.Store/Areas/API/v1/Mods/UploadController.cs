using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Archive;
using Disunity.Store.Backblaze;
using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Pages.Mods;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Schema;


namespace Disunity.Store.Areas.API.v1.Mods {

    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/mods/upload")]
    public class UploadController : ControllerBase {

        private readonly Func<IFormFile, Archive.Archive> _archiveFactory;

        private readonly ILogger<Upload> _logger;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly ApplicationDbContext _context;

        private readonly IB2Service _b2;

        public UploadController(ILogger<Upload> logger,
                                UserManager<UserIdentity> userManager,
                                ApplicationDbContext context,
                                Func<IFormFile, Archive.Archive> archiveFactory,
                                IB2Service b2) {
            _logger = logger;
            _userManager = userManager;
            _context = context;
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
        public async Task<IActionResult> PostAsync() {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            try {
                var archive = _archiveFactory(ArchiveUpload);

                _logger.LogInformation("File Upload received");

                if (_b2.ServiceConfigured) {
                    var user = await _userManager.GetUserAsync(HttpContext.User);

                    if (user == null) {
                        return Unauthorized();
                    }

                    var org = await _context.Orgs.SingleAsync(o => o.Slug == user.Slug);

                    var filename = $"{org.Slug}-{archive.Manifest.ModID}.zip";

                    var fileInfo = new Dictionary<string, string>() {{"modVersion", archive.Manifest.Version}};
                    _logger.LogInformation($"Uploading {filename}");

                    using (var uploadStream = _b2.GetUploadStream(filename, fileInfo)) {
                        await ArchiveUpload.CopyToAsync(uploadStream);
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