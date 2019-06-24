using Xunit;

using Disunity.Store.Shared.Util;
using Xunit.Abstractions;

namespace Disunity.Store.Tests {

    public class SchemaTests {

 
        private readonly ITestOutputHelper log;

        public SchemaTests(ITestOutputHelper log)
        {
            this.log = log;
        }
        
        [Fact]
        public void SchemaGenerated() {
            log.WriteLine(Schema.schema);
        }

    }

}