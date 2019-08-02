using System;
using System.Collections.Generic;

using BindingAttributes;

using Disunity.Store.Errors;
using Disunity.Store.Exceptions;
using Disunity.Store.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace Disunity.Store.Artifacts {

    public partial class Manifest {

        public string ModID;

        public string OrgID;
        public string URL;
        public string Version;

        public Dictionary<string, VersionRange> OptionalDependencies;
        public List<string> Artifacts;
        public int ContentTypes;
        public Dictionary<string, VersionRange> Dependencies;
        public string Description;

        public string DisplayName;

        public object ExtraData;

        public string Icon;
        public Dictionary<string, VersionRange> Incompatibilities;
        public List<string> PrefabBundles;

        public List<string> PreloadAssemblies;
        public string PreloadAssembly;
        public string PreloadClass;
        public string Readme;

        public List<string> RuntimeAssemblies;
        public string RuntimeAssembly;
        public string RuntimeClass;
        public List<string> SceneBundles;
        public List<string> Tags;
        public Dictionary<string, VersionRange> Targets;

        public VersionRange UnityVersions;

    }

    public partial class Manifest {

        private ILogger<Manifest> _logger;

        public Manifest() {
            Artifacts = new List<string>();
            Tags = new List<string>();
            UnityVersions = new VersionRange();
            Targets = new Dictionary<string, VersionRange>();
            Dependencies = new Dictionary<string, VersionRange>();
            OptionalDependencies = new Dictionary<string, VersionRange>();
            Incompatibilities = new Dictionary<string, VersionRange>();
        }

        /// <summary>
        /// Validate some JSON against the manifest JSONSchema.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="json"></param>
        /// <exception cref="ApiException"></exception>
        public static void ValidateJson(ILogger<Manifest> logger, string json) {
            try {
                var schema = Schema.LoadSchema();
                var obj = JObject.Parse(json);
                obj.IsValid(schema, out IList<ValidationError> errors);

                foreach (var error in errors) {
                    logger.LogError($"Schema error: {error.Message}");
                }

                if (errors.Count > 0) {
                    throw errors.AsAggregate().ToExec();
                }
            }
            catch (JsonReaderException e) {
                throw new ManifestParseError(e).ToExec();
            }
            
        }

        [Factory]
        public static Func<string, Manifest> ManifestFactory(IServiceProvider sp) {
            return json => {
                var logger = sp.GetRequiredService<ILogger<Manifest>>();
                ValidateJson(logger, json);
                var manifest = JsonConvert.DeserializeObject<Manifest>(json);
                manifest._logger = logger;
                return manifest;
            };
        }

    }

}