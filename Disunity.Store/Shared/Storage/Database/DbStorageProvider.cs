using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Disunity.Store.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Npgsql;


namespace Disunity.Store.Storage.Database {

    public class DbStorageProvider : IStorageProvider {

        private readonly ApplicationDbContext _context;

        public DbStorageProvider(ApplicationDbContext context) {
            _context = context;

        }

        public async Task<StorageFile> UploadFile(byte[] fileData, string filename,
                                                  Dictionary<string, string> fileInfo = null) {
            using (var uploadStream = GetUploadStream(filename, fileInfo)) {
                await uploadStream.WriteAsync(fileData);
                return uploadStream.FinalizeUpload();
            }
        }

        public async Task<StorageFile> UploadFile(Stream stream, string filename,
                                                  Dictionary<string, string> fileInfo = null) {
            using (var uploadStream = GetUploadStream(filename, fileInfo)) {
                await stream.CopyToAsync(uploadStream);
                return uploadStream.FinalizeUpload();
            }
        }

        public UploadStream GetUploadStream(string filename, Dictionary<string, string> fileInfo = null) {
            return new DbUploadStream(_context, filename, fileInfo);
        }

        public async Task<IActionResult> GetDownloadAction(string fileId) {
            var file = await _context.StoredFiles.FirstOrDefaultAsync(f => f.Guid == Guid.Parse(fileId));

            var manager = new NpgsqlLargeObjectManager(_context.Database.GetDbConnection() as NpgsqlConnection);

            var readStream = manager.OpenRead(file.ObjectId);

            return new FileStreamResult(readStream, "application/zip");
        }

    }

}