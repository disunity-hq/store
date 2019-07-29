using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Pages.Mods;
using Disunity.Store.Artifacts;
using Disunity.Store.Data;
using Disunity.Store.Exceptions;
using Disunity.Store.Storage;
using Disunity.Store.Storage.Backblaze;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Schema;

using AggregateException = Disunity.Store.Exceptions.AggregateException;


namespace Disunity.Store.Areas.API.v1.Mods {

    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/mods/upload")]
    public class UploadController : ControllerBase {

        private readonly Func<IFormFile, Archive> _archiveFactory;
        private readonly Func<Archive, Task<ModVersion>> _modVersionFactory;

        private readonly ILogger<Upload> _logger;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly ApplicationDbContext _context;

        private readonly IStorageProvider _storage;

        public UploadController(ILogger<Upload> logger,
                                UserManager<UserIdentity> userManager,
                                ApplicationDbContext context,
                                Func<IFormFile, Archive> archiveFactory,
                                Func<Archive, Task<ModVersion>> modVersionFactory,
                                IStorageProvider storage) {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _archiveFactory = archiveFactory;
            _modVersionFactory = modVersionFactory;
            _storage = storage;
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

                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null) {
                    return Unauthorized();
                }

                var uploadedFile = await _storage.UploadArchive(archive);

                var modVersion = await _modVersionFactory(archive);
                modVersion.FileId = uploadedFile.FileId;

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    $"orgSlug = {modVersion.Mod.Owner.Slug} modSlug = {modVersion.Mod.Slug} versionNumber = {modVersion.VersionNumber}");

                return new JsonResult(new {archive.Manifest.DisplayName});
            }
            catch (Exception e) {
                Exception[] errors = { e };

                if (e is AggregateException schemaExc) {
                    errors = schemaExc.ToArray();
                }

                return BadRequest(new {errors});
            }
        }

    }

}