using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Disunity.Store.Shared.Archive;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Overby.Extensions.AsyncBinaryReaderWriter;

namespace Disunity.Store.Areas.Mods.Pages {

    public class FileHelpers {

        public static string GetDisplayName(IFormFile formFile) {
            var fieldDisplayName = string.Empty;

            // Use reflection to obtain the display name for the model 
            // property associated with this IFormFile. If a display
            // name isn't found, error messages simply won't show
            // a display name.
            MemberInfo property =
                typeof(ArchiveUpload).GetProperty(
                    formFile.Name.Substring(formFile.Name.IndexOf(".") + 1));

            if (property == null) {
                return fieldDisplayName;
            }

            if (property.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute displayAttribute) {
                fieldDisplayName = $"{displayAttribute.Name} ";
            }

            return fieldDisplayName;
        }

        public static string GetFileName(IFormFile formFile) {
            // Use Path.GetFileName to obtain the file name, which will
            // strip any path information passed as part of the
            // FileName property. HtmlEncode the result in case it must 
            // be returned in an error message.
            return WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));
        }

        public static void CheckMimeType(IFormFile formFile,
                                         ModelStateDictionary modelState,
                                         string mimeType,
                                         string fieldDisplayName,
                                         string fileName) {
            if (formFile.ContentType.ToLower() != mimeType) {
                var message = $"The {fieldDisplayName}file ({fileName}) must be of type {mimeType}.";
                modelState.AddModelError(formFile.Name, message);
            }
        }

        public static bool CheckEmpty(IFormFile formFile,
                                      ModelStateDictionary modelState,
                                      string fieldDisplayName, string fileName) {
            if (formFile.Length == 0) {
                modelState.AddModelError(formFile.Name,
                                         $"The {fieldDisplayName} file ({fileName}) is empty.");
                return true;
            }

            return false;
        }

        public static bool CheckSize(IFormFile formFile,
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

        public static async Task<byte[]> GetFileContent(IFormFile formFile,
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

        public static async Task<Archive> ProcessFormFile(IFormFile formFile,
                                                          ModelStateDictionary modelState,
                                                          string mimeType) {
            var fieldDisplayName = GetDisplayName(formFile);
            var fileName = GetFileName(formFile);

            CheckMimeType(formFile, modelState, mimeType, fieldDisplayName, fileName);
            var empty = CheckEmpty(formFile, modelState, fieldDisplayName, fileName);
            var tooBig = CheckSize(formFile, modelState, fieldDisplayName, fileName);

            if (empty || tooBig) {
                return null;
            }


            try {
                var archive = new Archive(formFile.OpenReadStream());
                archive.Validate(formFile, modelState);
            }
            catch (Exception ex) {
                modelState.AddModelError(formFile.Name,
                                         $"The {fieldDisplayName}file ({fileName}) upload failed. " +
                                         $"Please contact the Discord for support. Error: {ex.Message}");
                // Log the exception
            }

            return null;
        }

    }

    public class ArchiveUpload {

        [Required]
        [Display(Name = "Mod Zip Archive")]
        public IFormFile Archive { get; set; }

    }

    public class Upload : PageModel {

        [BindProperty] public ArchiveUpload ArchiveUpload { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync() {
            // Perform an initial check to catch FileUpload class attribute violations.
            if (!ModelState.IsValid) {
                return Page();
            }

            var filePath = "<PATH-AND-FILE-NAME>";

            using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                await ArchiveUpload.Archive.CopyToAsync(fileStream);
            }

            return RedirectToPage("./Index");
        }

    }

}