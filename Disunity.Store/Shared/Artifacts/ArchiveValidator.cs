using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using BindingAttributes;

using Disunity.Store.Errors;
using Disunity.Store.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Artifacts {

    public static class ArchiveValidator {

        [Factory]
        public static Func<IFormFile, Archive> ArchiveFactory(IServiceProvider services) {
            var archiveFactory = services.GetRequiredService<Func<Stream, Archive>>();

            return formFile => {
                ValidateFormFile(formFile);
                var archive = archiveFactory(formFile.OpenReadStream());
                ValidateArtifacts(archive);
                return archive;
            };
        }

        private static string GetFileName(IFormFile formFile) {
            return WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));
        }

        private static void CheckMimeType(IFormFile formFile, string fileName, params string[] mimeTypes) {

            var fileMimeType = formFile.ContentType.ToLower();

            if (!mimeTypes.Contains(fileMimeType)) {
                var msg =
                    $"The file {fileName} must be of type {string.Join(", or ", mimeTypes.Select(s => $"`{s}`"))}. Type `{fileMimeType}` was provided";

                throw new InvalidArchiveError(msg).ToExec();
            }
        }

        private static void CheckEmpty(IFormFile formFile, string fileName) {
            if (formFile.Length == 0) {
                var msg = $"The file {fileName} is empty.";
                throw new InvalidArchiveError(msg).ToExec();
            }
        }

        private static void CheckSize(IFormFile formFile, string fileName) {
            if (formFile.Length > 1048576) {
                var msg = $"The file {fileName} exceeds 1 MB.";
                throw new InvalidArchiveError(msg).ToExec();
            }
        }

        private static void ValidateFormFile(IFormFile formFile) {
            var fileName = GetFileName(formFile);
            CheckMimeType(formFile, fileName, "application/zip", "application/x-zip", "application/x-zip-compressed");
            CheckEmpty(formFile, fileName);
            CheckSize(formFile, fileName);
        }

        private static void ValidateArtifacts(Archive archive) {
            var errors = (from path in archive.Manifest.Artifacts
                          let fullPath = Path.Combine("artifacts", path)
                          let entry = archive.GetEntry(fullPath)
                          where entry == null
                          select new MissingArtifactError(path))
                         .Cast<ApiError>()
                         .ToList();

            if (errors.Count > 0) {
                throw errors.AsAggregate().ToExec();
            }
        }
        

    }

}