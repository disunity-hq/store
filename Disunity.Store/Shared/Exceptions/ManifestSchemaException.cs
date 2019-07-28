using System;

using Newtonsoft.Json.Schema;


namespace Disunity.Store.Exceptions {

    public class ManifestSchemaException : Exception {

        public ManifestSchemaException(ValidationError[] errors) {
            Errors = errors;
        }

        public ValidationError[] Errors { get; }

    }

}