using System;
using System.IO;
using System.IO.Compression;
using System.Text;

using Disunity.Store.Code.Startup;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Code.Archive {

    public class ArchiveLoadException : Exception {

        public ArchiveLoadException(string message) : base(message) { }

    }

    public class Archive {

        private ZipArchive archive;

        private ILogger<Archive> log;
        private Func<string, Manifest> manifestFactory;

        public Archive(ILogger<Archive> log,
                       Func<string, Manifest> manifestFactory,
                       Stream stream) {
            this.log = log;
            this.manifestFactory = manifestFactory;
            archive = new ZipArchive(stream, ZipArchiveMode.Read);
            Manifest = GetManifest();
            Readme = GetReadme();
        }


        public Manifest Manifest { get; }
        public string Readme { get; }

        [Factory]
        public static Func<Stream, Archive> StreamArchiveFactory(IServiceProvider services) {
            var logger = services.GetRequiredService<ILogger<Archive>>();
            var manifestFactory = services.GetRequiredService<Func<string, Manifest>>();
            return stream => new Archive(logger, manifestFactory, stream);
        }


        public ZipArchiveEntry GetEntry(string filename) {
            var entry = archive.GetEntry(filename);
            return entry;
        }

        public Manifest GetManifest(string filename = "manifest.json") {
            var entry = GetEntry(filename);

            using (var file = entry.Open()) {
                var reader = new StreamReader(file);
                var json = reader.ReadToEnd();
                return manifestFactory(json);
            }
        }

        public string GetReadme(string filename = "README.md") {
            var entry = GetEntry(filename);
            var encoding = new UTF8Encoding(false, true);

            using (var reader = new StreamReader(entry.Open(), encoding, true)) {
                var readme = reader.ReadToEnd();

                if (readme.Length == 0) {
                    var message = $"Readme file, {filename} is empty.";
                    throw new ArchiveLoadException(message);
                }

                return readme;
            }
        }

    }

}