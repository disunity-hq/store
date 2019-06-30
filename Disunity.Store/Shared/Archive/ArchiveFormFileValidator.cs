using System;
using System.IO;
using System.Net;
using Disunity.Store.Shared.Startup;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Disunity.Store.Shared.Archive {

    public class ArchiveFormFileValidationError : Exception {

        public ArchiveFormFileValidationError(string message) : base(message) { }

    }

    public static class ArchiveFormFileValidator {

        [Factory]
        public static Func<IFormFile, Archive> ArchiveFactory(IServiceProvider services) {
            var archiveFactory = services.GetRequiredService<Func<Stream, Archive>>();
            return formFile => {
                ValidateFormFile(formFile);
                return archiveFactory(formFile.OpenReadStream());
            };
        }

        private static void ValidateFormFile(IFormFile formFile) {
            var fileName = GetFileName(formFile);
            CheckMimeType(formFile, "application/zip", fileName);
            CheckEmpty(formFile, fileName);
            CheckSize(formFile, fileName);
        }

        private static string GetFileName(IFormFile formFile) {
            return WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));
        }

        private static void CheckMimeType(IFormFile formFile, string mimeType, string fileName) {
            if (formFile.ContentType.ToLower() != mimeType) {
                var msg = $"The file {fileName} must be of type {mimeType}.";
                throw new ArchiveFormFileValidationError(msg);
            }
        }

        private static void CheckEmpty(IFormFile formFile, string fileName) {
            if (formFile.Length == 0) {
                var msg = $"The file {fileName} is empty.";
                throw new ArchiveFormFileValidationError(msg);
            }
        }

        private static void CheckSize(IFormFile formFile, string fileName) {
            if (formFile.Length > 1048576) {
                var msg = $"The file {fileName} exceeds 1 MB.";
                throw new ArchiveFormFileValidationError(msg);
            }
        }

    }

}