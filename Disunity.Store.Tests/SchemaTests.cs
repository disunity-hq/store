using System.Collections.Generic;
using System.IO;

using Disunity.Store.Code.Archive;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

using Xunit;
using Xunit.Abstractions;


namespace Disunity.Store.Tests {

    public class SchemaTests {

        public SchemaTests(ITestOutputHelper log) {
            this.log = log;
        }


        private readonly ITestOutputHelper log;

        [Fact]
        public void SchemaGenerated() {
            var schema = Schema.LoadSchema();
            var manifest_json = File.ReadAllText("data/broken_manifest.json");
            var manifest = JObject.Parse(manifest_json);
            Assert.False(manifest.IsValid(schema, out IList<ValidationError> errors));

            foreach (var error in errors) {
                Assert.Equal("ModID", error.Path);

                Assert.Equal("String 'example mod' does not match regex pattern '^[a-z0-9]+(?:-[a-z0-9]+)*$'.",
                             error.Message);
            }
        }

    }

}