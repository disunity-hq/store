using B2Net;
using B2Net.Models;
using BindingAttributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disunity.Store.Shared.Data.Services {
    public interface IB2Service {
        Task UploadFile(byte[] fileData, string filename);
    }

    [AsSingleton(typeof(IB2Service))]
    public class B2Service : IB2Service {
        private readonly ILogger<B2Service> _logger;

        private B2Client _client;

        public B2Service(IConfiguration config, ILogger<B2Service> logger) {
            var options = new B2Options() {
                AccountId = config["Auth:B2:AccountId"],
                KeyId = config["Auth:B2:KeyId"],
                ApplicationKey = config["Auth:B2:AppKey"],
                BucketId = config["Auth:B2:BucketId"],
                PersistBucket = true
            };

            // if backblaze isn't fully configured
            if (string.IsNullOrEmpty(options.AccountId) ||
                string.IsNullOrEmpty(options.KeyId) ||
                string.IsNullOrEmpty(options.ApplicationKey) ||
                string.IsNullOrEmpty(options.BucketId)) {
                // And it's at least partially configured
                if (!string.IsNullOrEmpty(options.AccountId) ||
                    !string.IsNullOrEmpty(options.KeyId) ||
                    !string.IsNullOrEmpty(options.ApplicationKey) ||
                    !string.IsNullOrEmpty(options.BucketId))
                    throw new InvalidOperationException("Backblaze not fully configured.");
            }

            _client = new B2Client(options);
            _logger = logger;
        }

        public async Task UploadFile(byte[] fileData, string filename) {
            var file = await _client.Files.Upload(fileData, filename);
            if (file == null) {
                return;
            }
            foreach (var entry in file.FileInfo) {
                _logger.LogInformation($"File info key: {entry.Key} value: {entry.Value}");
            }
        }

    }
}
