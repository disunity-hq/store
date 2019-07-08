using System;
using System.Collections.Generic;
using System.Linq;

using Disunity.Store.Shared.Startup;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

using SemVersion;


namespace Disunity.Store.Shared.Archive {

    public class ManifestSchemaException : Exception {

        public ManifestSchemaException(ValidationError[] errors) {
            Errors = errors;
        }

        public ValidationError[] Errors { get; }

    }

    public class VersionRange {

        public string MaxVersion = null;

        public string MinVersion = null;

        public VersionRange(string min = null, string max = null) {
            MinVersion = min;
            MaxVersion = max;
        }

        public override string ToString() {
            return $"{MinVersion} - {MaxVersion}";
        }

        public override bool Equals(object obj) {
            return obj is VersionRange range &&
                   MinVersion == range.MinVersion &&
                   MaxVersion == range.MaxVersion;
        }

        public string Validate() {
            if (MinVersion == null || MaxVersion == null) {
                return null;
            }

            SemanticVersion min = MinVersion;
            SemanticVersion max = MaxVersion;

            if (min < max) {
                return null;
            }

            return "MinVersion is higher than or equal to MaxVersion";
        }

    }

    public partial class Manifest {

        public int ContentTypes;
        public Dictionary<string, VersionRange> Dependencies;
        public string Description;
        public string ModID;

        public string Name;

        public string OrgID;

        public List<string> PreloadAssemblies;

        public string PreloadAssembly;
        public string PreloadClass;
        public List<string> RuntimeAssemblies;

        public string RuntimeAssembly;
        public string RuntimeClass;

        public Dictionary<string, VersionRange> Targets;
        public string Url;
        public string Version;

    }


    public partial class Manifest {

        public ILogger<Manifest> logger;

        public static void ValidateJson(ILogger<Manifest> logger, string json) {
            logger.LogError("Manifest logger working.");
            var schema = Schema.LoadSchema();
            var obj = JObject.Parse(json);
            obj.IsValid(schema, out IList<ValidationError> errors);

            foreach (var error in errors) {
                logger.LogError($"Schema error: {error.Message}");
            }

            if (errors.Count > 0) {
                throw new ManifestSchemaException(errors.ToArray());
            }
        }

        [Factory]
        public static Func<string, Manifest> ManifestFactory(IServiceProvider sp) {
            return json => {
                var logger = sp.GetRequiredService<ILogger<Manifest>>();
                ValidateJson(logger, json);
                var manifest = JsonConvert.DeserializeObject<Manifest>(json);
                manifest.logger = logger;
                return manifest;
            };
        }

    }

}