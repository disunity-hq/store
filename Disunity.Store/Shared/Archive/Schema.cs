using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace Disunity.Store.Shared.Archive {

    public static partial class Schema {

        private const string SLUG_PATTERN = "^[a-z0-9]+(?:-[a-z0-9]+)*$";
        private const string NAME_PATTERN = "^[a-zA-Z0-9 -_]+$";
        public const string WORD_PATTERN = "^[a-zA-Z0-9-_]+$";
        private const string CLASS_PATTERN = "^[a-zA-Z]+$";
        private const string PACKAGE_PATTERN = "^[a-z0-9]+(?:-[a-z0-9]+)*/[a-z0-9]+(?:-[a-z0-9]+)*$";
        private const string VERSION_PATTERN = "^[0-9]+\\.[0-9]+\\.[0-9]+$";
        private const string DLL_PATTERN = "^[a-zA-Z0-9-_]+\\.dll$";

        private static object SchemaData() {
            return Object(new {
                // identifiers
                OrgID = String(SLUG_PATTERN),
                ModID = String(SLUG_PATTERN),
                Version = String(VERSION_PATTERN),

                // information
                Name = String(NAME_PATTERN),
                Url = String(format: "url"),
                Description = String(),
                ContentTypes = new {type = "integer"},

                // relations
                Targets = Object(
                    propertyNames: new {pattern = SLUG_PATTERN},
                    additionalProps: Object(new {
                        MinVersion = String(),
                        MaxVersion = String()
                    })),

                Dependencies = Object(
                    propertyNames: new {pattern = PACKAGE_PATTERN},
                    additionalProps: Object(new {
                        MinVersion = String(),
                        MaxVersion = String()
                    })),

                // assets
                Artifacts = Array(String()),

                PreloadAssemblies = Array(String(DLL_PATTERN)),
                PreloadAssembly = String(CLASS_PATTERN),
                PreloadClass = String(CLASS_PATTERN),

                RuntimeAssemblies = Array(String(DLL_PATTERN)),
                RuntimeAssembly = String(CLASS_PATTERN),
                RuntimeClass = String(CLASS_PATTERN)
            }, dependencies: new {
                PreloadClass = new[] {"PreloadAssemblies", "PreloadAssembly"},
                PreloadAssembly = new[] {"PreloadAssemblies", "PreloadClass"},
                RuntimeClass = new[] {"RuntimeAssemblies", "RuntimeAssembly"},
                RuntimeAssembly = new[] {"RuntimeAssemblies", "RuntimeClass"}
            }, required: new[] {
                "OrgID", "ModID", "Name", "Url", "Version", "Description", "Targets", "ContentTypes"
            });
        }

        private static string SchemaJson(object data = null) {
            if (data == null) {
                data = SchemaData();
            }

            return JsonConvert.SerializeObject(data);
        }

        public static JSchema LoadSchema(string json = null) {
            if (json == null) {
                json = SchemaJson();
            }

            var schema = JSchema.Parse(json);
            schema.SchemaVersion = new Uri("http://json-schema.org/draft-04/schema#");
            return schema;
        }

        public static bool ValidateJson(string json, string schemaJson = null) {
            var validator = LoadSchema(schemaJson);
            var obj = JObject.Parse(json);
            return obj.IsValid(validator);
        }

    }

}