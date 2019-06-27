using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Disunity.Store.Shared.Archive {

    public class Archive {

        private ZipArchive archive;

        public Manifest Manifest { get; protected set; }
        public string Readme { get; protected set; }

        public Archive(System.IO.Stream stream) {
            var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        }

        public ZipArchiveEntry GetEntry(string filename) {
            return archive.GetEntry(filename);
        }

        public Manifest GetManifest(IFormFile formFile,
                                    ModelStateDictionary modelState, 
                                    string filename = "manifest.json") {

            var entry = GetEntry(filename);
            using (var file = entry.Open()) {
                var reader = new StreamReader(file);
                var json = reader.ReadToEnd();
                var schema_errors = Manifest.ValidateJson(json);

                foreach (var error in schema_errors) {
                    modelState.AddModelError(formFile.Name, error.Message);
                }

                if (schema_errors.Count == 0) {
                    return Manifest.FromJson(json);
                }

                return null;
            }
        }

        public string GetReadme(IFormFile formFile,
                                ModelStateDictionary modelState, 
                                string filename = "README.md") {
            var entry = GetEntry(filename);
            var encoding = new UTF8Encoding(false, true);
            using (var reader = new StreamReader(entry.Open(), encoding, true)) {
                var readme = reader.ReadToEnd();

                if (readme.Length == 0) {
                    var message = $"Readme file, {filename} is empty.";
                    modelState.AddModelError(formFile.Name, message);
                }

                return readme;
            }
        }

        public void Validate(IFormFile formFile, 
                             ModelStateDictionary modelState) {
            Manifest = GetManifest(formFile, modelState);
            Readme = GetReadme(formFile, modelState);
        }

    }

}