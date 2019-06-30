using System;
using System.Collections.Generic;
using System.Linq;
using Disunity.Store.Shared.Startup;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using SemVersion;
using SemVersion.Parser;

namespace Disunity.Store.Shared.Archive {

    public class ManifestSchemaException : Exception {

        public ValidationError[] Errors { get; }

        public ManifestSchemaException(ValidationError[] errors) {
            Errors = errors;
        }

    }

    public class VersionRange {

        public string MinVersion = null;
        public string MaxVersion = null;

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

        public string OrgID;
        public string ModID;
        public string Version;

        public string Name;
        public string Url;
        public string Description;
        public int ContentTypes;

        public Dictionary<string, VersionRange> Targets;
        public Dictionary<string, VersionRange> Dependencies;

        public List<string> PreloadAssemblies;
        public List<string> RuntimeAssemblies;

        public string PreloadAssembly;
        public string PreloadClass;

        public string RuntimeAssembly;
        public string RuntimeClass;

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