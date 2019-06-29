using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Disunity.Store.Shared.Startup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Overby.Extensions.AsyncBinaryReaderWriter;

namespace Disunity.Store.Shared.Archive {

    [Binding(BindType.Singleton)]
    public class ArchiveValidator {

        private ILogger<ArchiveValidator> log;
        private Func<Stream, Archive> archiveFactory;
        private string message;

        public ArchiveValidator(ILogger<ArchiveValidator> log, Func<Stream, Archive> archiveFactory) {
            this.log = log;
            this.archiveFactory = archiveFactory;
        }

        public async Task<Archive> ValidateAsync(IFormFile formFile,
                                                 ModelStateDictionary modelState,
                                                 string mimeType) {
            var fieldDisplayName = GetDisplayName(formFile);
            var fileName = GetFileName(formFile);

            CheckMimeType(formFile, modelState, mimeType, fieldDisplayName, fileName);
            var empty = CheckEmpty(formFile, modelState, fieldDisplayName, fileName);
            var tooBig = CheckSize(formFile, modelState, fieldDisplayName, fileName);

            if (empty) {
                return null;
            } 
            
            if (tooBig) {
                return null;
            }

            try {
                var stream = formFile.OpenReadStream();
                var archive = archiveFactory(stream);
                archive.Validate(formFile, modelState);
                return archive;
            }
            catch (Exception ex) {
                var message = $"The {fieldDisplayName}file ({fileName}) upload failed. " +
                              $"Please contact the Discord for support. Error: {ex.Message}";
                modelState.AddModelError(formFile.Name, message);
                log.LogWarning(message);
                return null;
            }
        }

        protected static string GetDisplayName(IFormFile formFile) {
            var fieldDisplayName = string.Empty;
            return fieldDisplayName;
        }

        protected static string GetFileName(IFormFile formFile) {
            // Use Path.GetFileName to obtain the file name, which will
            // strip any path information passed as part of the
            // FileName property. HtmlEncode the result in case it must 
            // be returned in an error message.
            return WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));
        }

        protected static void CheckMimeType(IFormFile formFile,
                                            ModelStateDictionary modelState,
                                            string mimeType,
                                            string fieldDisplayName,
                                            string fileName) {
            if (formFile.ContentType.ToLower() != mimeType) {
                var message = $"The {fieldDisplayName}file ({fileName}) must be of type {mimeType}.";
                modelState.AddModelError(formFile.Name, message);
            }
        }

        protected static bool CheckEmpty(IFormFile formFile,
                                         ModelStateDictionary modelState,
                                         string fieldDisplayName, string fileName) {
            if (formFile.Length == 0) {
                modelState.AddModelError(formFile.Name,
                                         $"The {fieldDisplayName} file ({fileName}) is empty.");
                return true;
            }

            return false;
        }

        protected static bool CheckSize(IFormFile formFile,
                                        ModelStateDictionary modelState,
                                        string fieldDisplayName,
                                        string fileName) {
            if (formFile.Length > 1048576) {
                modelState.AddModelError(formFile.Name,
                                         $"The {fieldDisplayName}file ({fileName}) exceeds 1 MB.");
                return true;
            }

            return false;
        }

        protected static async Task<byte[]> GetFileContent(IFormFile formFile,
                                                           ModelStateDictionary modelState,
                                                           string fieldDisplayName,
                                                           string fileName) {
            byte[] content;

            // The StreamReader is created to read files that are UTF-8 encoded. 
            // If uploads require some other encoding, provide the encoding in the 
            // using statement. To change to 32-bit encoding, change 
            // new UTF8Encoding(...) to new UTF32Encoding().
            using (var reader = new AsyncBinaryReader(formFile.OpenReadStream())) {
                content = await reader.ReadBytesAsync(int.MaxValue);

                if (content.Length > 0) {
                    return content;
                }

                var error = $"The {fieldDisplayName}file ({fileName}) is empty.";
                modelState.AddModelError(formFile.Name, error);
            }

            return content;
        }

    }

}