using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Disunity.Store.Shared.Startup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disunity.Store.Shared.Archive {

    public class Archive {

        private ZipArchive archive;

        public Manifest Manifest { get; protected set; }
        public string Readme { get; protected set; }

        private ILogger<Archive> log;

        public Archive(ILogger<Archive> log, Stream stream) {
            this.log = log;
            archive = new ZipArchive(stream, ZipArchiveMode.Read);
        }

        [Factory]
        public static Func<Stream, Archive> ArchiveFactory(IServiceProvider services) {
            var logger = services.GetRequiredService<ILogger<Archive>>();
            return stream => new Archive(logger, stream);
        }

        public ZipArchiveEntry GetEntry(string filename) {
            var entry = archive.GetEntry(filename);
            return entry;
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
            if (archive == null) {
                modelState.AddModelError(formFile.Name, "Archive was null");
                return;
            }

            Manifest = GetManifest(formFile, modelState);
            Readme = GetReadme(formFile, modelState);
        }

    }

}