using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Store.Artifacts;
using Disunity.Store.Entities;


namespace Disunity.Store.Storage {

    public static class StorageProviderExtensions {

        public static async Task<StorageFile> UploadArchive(this IStorageProvider storageProvider, Archive archive) {
            var filename = $"{archive.Manifest.OrgID}-{archive.Manifest.ModID}.zip";

            var fileInfo = new Dictionary<string, string>() {{"modVersion", archive.Manifest.Version}};

            using (var uploadStream = storageProvider.GetUploadStream(filename, fileInfo)) {
                await archive.CopyToAsync(uploadStream);
                return uploadStream.FinalizeUpload();
            }
        }

    }

}