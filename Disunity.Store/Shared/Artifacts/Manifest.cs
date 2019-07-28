using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using BindingAttributes;

using Disunity.Store.Exceptions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace Disunity.Store.Artifacts {

    public partial class Manifest {

        public string OrgID;
        public string ModID;
        public string Version;

        public string DisplayName;
        public string URL;
        public string Description;
        public List<string> Tags;
        public int ContentTypes;

        public VersionRange UnityVersions;
        public Dictionary<string, VersionRange> Targets;
        public Dictionary<string, VersionRange> Dependencies;
        public Dictionary<string, VersionRange> OptionalDependencies;
        public Dictionary<string, VersionRange> Incompatibilities;

        public string Icon;
        public string Readme;
        public List<string> Artifacts;
        public List<string> PrefabBundles;
        public List<string> SceneBundles;

        public List<string> PreloadAssemblies;
        public string PreloadAssembly;
        public string PreloadClass;

        public List<string> RuntimeAssemblies;
        public string RuntimeAssembly;
        public string RuntimeClass;

        public object ExtraData;

    }


    public partial class Manifest {

        public ILogger<Manifest> logger;

        public Manifest() {
            Tags = new List<string>();
            UnityVersions = new VersionRange();
            Targets = new Dictionary<string, VersionRange>();
            Dependencies = new Dictionary<string, VersionRange>();
            OptionalDependencies = new Dictionary<string, VersionRange>();
            Incompatibilities = new Dictionary<string, VersionRange>();
        }

        public static void ValidateJson(ILogger<Manifest> logger, string json) {
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