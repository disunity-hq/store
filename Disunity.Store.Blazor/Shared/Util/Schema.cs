using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Disunity.Store.Shared.Util {

    public static class Schema {

        public static string schema = Schema.GenerateSchema();

        public static readonly string SLUG_PATTERN = "^[a-z0-9]+(?:-[a-z0-9]+)*$";
        public static readonly string NAME_PATTERN = "^[a-zA-Z0-9 -_]+$";
        public static readonly string WORD_PATTERN = "^[a-zA-Z0-9-_]+$";
        public static readonly string CLASS_PATTERN = "^[a-zA-Z]+$";
        public static readonly string PACKAGE_PATTERN = "^[a-z0-9]+(?:-[a-z0-9]+)*/[a-z0-9]+(?:-[a-z0-9]+)*$";
        public static readonly string VERSION_PATTERN = "^[0-9]+\\.[0-9]+\\.[0-9]+$";
        public static readonly string DLL_PATTERN = "^[a-zA-Z0-9-_]+\\.dll$";

        public static ExpandoObject OrgIdSchema() {
            dynamic eo = new ExpandoObject();
            eo.type = "string";
            eo.maxLength = 128;
            eo.pattern = SLUG_PATTERN;
            return eo;
        }

        public static string GenerateSchema() {
            dynamic schema = new ExpandoObject();
            schema.type = "object";
            schema.properties = new ExpandoObject();
            schema.properties.OrgID = OrgIdSchema();
            return JsonConvert.SerializeObject(schema);
        }

    }

}