using B2Net;
using B2Net.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Disunity.Store.Shared.Backblaze {

    public class B2UploadStream : Stream {

        private const int MinPartSize = 5 * 1024 * 1024;

        private readonly B2Client _client;
        private readonly string _fileName;
        private readonly string _bucketId;
        private readonly Dictionary<string, string> _fileInfo;
        private MemoryStream _buffer;

        private int _partCount = 0;

        private B2File _start;
        private B2File _finish;

        private readonly List<string> _shas = new List<string>();

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => _finish == null;

        public override long Length => throw new NotSupportedException("B2UploadStream only supports writing");

        public override long Position {
            get => throw new NotSupportedException("B2UploadStream only supports writing");
            set => throw new NotSupportedException("B2UploadStream only supports writing");
        }

        public B2UploadStream(B2Client client, string fileName) : this(client, fileName, bucketId: "") { }

        public B2UploadStream(B2Client client, string fileName, string bucketId) : this(
            client, fileName, bucketId, null) { }

        public B2UploadStream(B2Client client, string fileName, Dictionary<string, string> fileInfo) : this(
            client, fileName, "", fileInfo) { }

        public B2UploadStream(B2Client client, string fileName, string bucketId, Dictionary<string, string> fileInfo) {
            _client = client;
            _fileName = fileName;
            _bucketId = bucketId;
            _fileInfo = fileInfo;

            _buffer = new MemoryStream(MinPartSize);
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (!disposing) {
                return;
            }

            _buffer.Dispose();

            FinalizeUpload();
        }

        public override void Flush() {
            if (_finish != null) {
                throw new InvalidOperationException(
                    "Cannot flush after last part has been written. Last part gets sent on the first flush of <5MB");
            }

            if (_start == null) {
                _start = _client.LargeFiles.StartLargeFile(_fileName, "", _bucketId, _fileInfo).Result;
            }

            var partSize = _buffer.Length;
            if (partSize == 0) return;

            var partBytes = _buffer.ToArray();

            _shas.Add(GetSHA1Hash(partBytes));

            _buffer.Dispose();
            _buffer = new MemoryStream();

            try {

                var uploadUrl = _client.LargeFiles.GetUploadPartUrl(_start.FileId).Result;
                _client.LargeFiles.UploadPart(partBytes, _partCount + 1, uploadUrl).Wait();

                if (partSize < MinPartSize) {
                    FinalizeUpload();
                }
            }
            catch (Exception e) {
                _client.LargeFiles.CancelLargeFile(_start.FileId).Wait();
                throw;
            }
        }

        public override int Read(byte[] buffer, int offset, int count) {
            throw new NotSupportedException("B2UploadStream only supports writing");
        }

        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotSupportedException("B2UploadStream only supports writing");
        }

        public override void SetLength(long value) {

            throw new NotSupportedException("B2UploadStream only supports writing");
        }

        public override void Write(byte[] buffer, int offset, int count) {
            _buffer.Write(buffer, offset, count);
            CheckFlush();
        }

        public override void WriteByte(byte value) {
            _buffer.WriteByte(value);
            CheckFlush();
        }

        private void CheckFlush() {
            if (_buffer.Length > MinPartSize) {
                Flush();
            }
        }

        private string GetSHA1Hash(byte[] bytes) {
            using (var sha1 = SHA1.Create()) {
                var hashBytes = sha1.ComputeHash(bytes);

                var sb = new StringBuilder();

                foreach (byte b in hashBytes) {
                    var hex = b.ToString("x2");
                    sb.Append(hex);
                }

                return sb.ToString();
            }
        }

        public B2File FinalizeUpload() {
            if (_finish != null) return _finish;

            if (_start == null) {
                var bytes = _buffer.ToArray();
                _finish = _client.Files.Upload(bytes, _fileName, _bucketId, _fileInfo).Result;
            } else {
                try {
                    _finish = _client.LargeFiles.FinishLargeFile(_start.FileId, _shas.ToArray()).Result;
                }
                catch (Exception e) {
                    _client.LargeFiles.CancelLargeFile(_start.FileId).Wait();
                    throw;
                }
            }

            return _finish;
        }

    }

}