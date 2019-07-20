using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Extensions;
using Disunity.Store.Storage;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Middleware {

    public class DownloadRedirectMiddleware {

        private readonly RequestDelegate _next;
        private readonly IStorageProvider _storage;
        private readonly ILogger<DownloadRedirectMiddleware> _logger;

        public DownloadRedirectMiddleware(RequestDelegate next, IStorageProvider storage,
                                          ILogger<DownloadRedirectMiddleware> logger) {
            _next = next;
            _storage = storage;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext) {
            var regex = new Regex(
                @"/mods/download/(?'owner'[a-z\d]+(?:-[a-z\d]+)*)/(?'mod'[a-z\d]+(?:-[a-z\d]+)*)/(?'version'\d+\.\d\.\d)\.zip");

            var match = regex.Match(context.Request.Path);

            if (match.Success) {
                var ownerSlug = match.Groups["owner"].Value;
                var modSlug = match.Groups["mod"].Value;
                var versionString = match.Groups["version"].Value;

                _logger.LogInformation($"Download request for {ownerSlug}/{modSlug}@{versionString}");

                var modVersion = await dbContext.ModVersions
                                                .Where(v => v.Mod.Slug == modSlug && v.Mod.Owner.Slug == ownerSlug)
                                                .FindExactVersion(new VersionNumber(versionString))
                                                .FirstOrDefaultAsync();

                if (modVersion != null) {
                    _logger.LogInformation(
                        $"Received download request for {modVersion.DisplayName}@{modVersion.VersionNumber}");

                    var downloadAction = await _storage.GetDownloadAction(modVersion.FileId);

                    switch (downloadAction) {
                        case RedirectResult redirectResult:
                            await context.ExecuteResultAsync(redirectResult);
                            break;

                        case FileResult fileResult:
                            await context.ExecuteResultAsync(fileResult);
                            break;
                    }

                    return;
                }

            }

            await _next(context);
        }

    }

}