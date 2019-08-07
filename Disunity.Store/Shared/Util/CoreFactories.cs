using System;
using System.IO;

using BindingAttributes;

using Disunity.Core.Archives;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;


namespace Disunity.Store.Util {

    public class CoreFactories {

        [Factory]
        public static Func<string, Manifest> ManifestFactory(IServiceProvider sp) {
            return json => {
                Manifest.ValidateJson(json);
                var manifest = JsonConvert.DeserializeObject<Manifest>(json);
                return manifest;
            };
        }
        
        [Factory]
        public static Func<Stream, ZipArchive> StreamArchiveFactory(IServiceProvider services) {
            var manifestFactory = services.GetRequiredService<Func<string, Manifest>>();
            return stream => new ZipArchive(manifestFactory, stream);
        }

    }

}