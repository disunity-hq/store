using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Errors;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Artifacts {

    public class ZipArchive : BaseArchive {

        private readonly Stream _stream;
        private readonly System.IO.Compression.ZipArchive _zip;

        private Manifest _manifest;
        private string _readme;

        public ZipArchive(Func<string, Manifest> manifestFactory, Stream stream) : base(manifestFactory) {
            _stream = stream;
            _zip = new System.IO.Compression.ZipArchive(stream, ZipArchiveMode.Read);
        }

        [Factory]
        public static Func<Stream, ZipArchive> StreamArchiveFactory(IServiceProvider services) {
            var manifestFactory = services.GetRequiredService<Func<string, Manifest>>();
            return stream => new ZipArchive(manifestFactory, stream);
        }

        public Manifest Manifest => _manifest ?? (_manifest = GetManifest());
        public string Readme => _readme ?? (_readme = GetReadmeContent());

        public override bool HasEntry(string filename) {
            return _zip.GetEntry(filename) != null;
        }
        
        public override Stream GetEntry(string filename) {
            var entry = _zip.GetEntry(filename);
            return entry?.Open();
        }

        public override IEnumerable<string> ArtifactPaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("artifacts", StringComparison.Ordinal))
                    .Select(e => e.FullName);

        public override IEnumerable<string> PreloadAssemblyPaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("preload", StringComparison.Ordinal))
                    .Select(e => e.FullName);

        public override IEnumerable<string> RuntimeAssemblyPaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("runtime", StringComparison.Ordinal))
                    .Select(e => e.FullName);

        public override IEnumerable<string> PrefabBundlePaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("prefabs", StringComparison.Ordinal))
                    .Select(e => e.FullName);

        public override IEnumerable<string> SceneBundlePaths =>
            _zip.Entries
                    .Where(e => e.FullName.StartsWith("scenes", StringComparison.Ordinal))
                    .Select(e => e.FullName);

        public string GetReadmeContent() {
            var stream = GetReadme();

            if (stream == null) {
                return "";
            }

            var encoding = new UTF8Encoding(false, true);

            using (var reader = new StreamReader(stream, encoding, true)) {
                var readme = reader.ReadToEnd();

                if (readme.Length == 0) {
                    var message = $"Readme file is empty.";
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