using B2Net;
using B2Net.Models;


namespace Disunity.Store.Storage.Backblaze {

    public static class B2StorageFile {

        public static StorageFile Create(B2Client b2Client, B2File file, string bucketName) {
            if (file == null) {
                return null;
            }

            var downloadUrl = b2Client.Files.GetFriendlyDownloadUrl(file.FileName, bucketName);

            return new StorageFile() {
                DownloadUrl = downloadUrl,
                FileName = file.FileName,
                Metadata = file.FileInfo
            };
        }

    }

}