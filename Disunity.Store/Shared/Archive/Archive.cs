using System.IO;
using System.IO.Compression;

namespace Disunity.Store.Shared.Archive {

    public class Archive {

        private ZipArchive archive;

        public Archive(System.IO.Stream stream) {
            var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        }

        public ZipArchiveEntry GetEntry(string filename) {
            return archive.GetEntry(filename);
        }

        public Manifest GetManifest(string filename = "manifest.json") {
            var entry = GetEntry(filename);
            using (var file = entry.Open()) {
                var reader = new StreamReader(file);
                var json = reader.ReadToEnd();
                return Manifest.FromJson(json);
            }
        }
        
    }

}