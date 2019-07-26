using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Disunity.Store.Artifacts;
using Disunity.Store.Entities;
using Disunity.Store.Storage.Backblaze;

using Microsoft.AspNetCore.Mvc;


namespace Disunity.Store.Storage {

    public interface IStorageProvider {

        Task<StorageFile> UploadFile(byte[] fileData, string filename, Dictionary<string, string> fileInfo = null);

        Task<StorageFile> UploadFile(Stream stream, string filename, Dictionary<string, string> fileInfo = null);

        UploadStream GetUploadStream(string filename, Dictionary<string, string> fileInfo = null);
        
        Task<IActionResult> GetDownloadAction(string fileId);

        Task DeleteFile(string fileId);

    }

}