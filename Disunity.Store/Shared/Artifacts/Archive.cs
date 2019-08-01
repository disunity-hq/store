using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Errors;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Artifacts {

    public class Archive {

        private readonly ZipArchive _archive;
        private readonly Func<string, Manifest> _manifestFactory;
        private readonly Stream _stream;

        private ILogger<Archive> _log;

        public Archive(ILogger<Archive> log,
                       Func<string, Manifest> manifestFactory,
                       Stream stream) {
            _log = log;
            _manifestFactory = manifestFactory;
            _stream = stream;
            _archive = new ZipArchive(stream, ZipArchiveMode.Read);
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
            var entry = _archive.GetEntry(filename);
            return entry;
        }

        public Manifest GetManifest(string filename = "manifest.json") {

            var entry = GetEntry(filename);

            if (entry == null) {
                throw new ArchiveLoadError($"No manifest found at {filename}").ToExec();
            }

            using (var file = entry.Open()) {
                var reader = new StreamReader(file);
                var json = reader.ReadToEnd();
                return _manifestFactory(json);
            }
        }

        public string GetReadme(string filename = null) {
            if (filename == null) {
                filename = Manifest.Readme ?? "Readme.md";
            }

            var entry = GetEntry(filename);

            if (entry == null) {
                return "";
            }

            var encoding = new UTF8Encoding(false, true);

            using (var reader = new StreamReader(entry.Open(), encoding, true)) {
                var readme = reader.ReadToEnd();

                if (readme.Length == 0) {
                    var message = $"Readme file, {filename} is empty.";
                    throw new ArchiveLoadError(message).ToExec();
                }

                return readme;
            }
        }

        public void CopyTo(Stream stream) {
            _stream.Seek(0, SeekOrigin.Begin);
            _stream.CopyTo(stream);
        }

        public Task CopyToAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken)) {
            _stream.Seek(0, SeekOrigin.Begin);
            return _stream.CopyToAsync(stream, cancellationToken);
        }

    }

}