using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;


namespace Disunity.Store.Exceptions {

    public class SchemaException : BaseApiException {

        [JsonProperty] public int LineNumber { get; }
        [JsonProperty] public int LinePosition { get; }
        [JsonProperty] public string Path { get; }
        [JsonProperty] public object Value { get; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorType ErrorType { get; }

        public SchemaException(string message, int lineNumber, int linePosition,
                               string path, object value, ErrorType errorType,
                               string name = null, string context = null)
            : base(message, name, context) {
            LineNumber = lineNumber;
            LinePosition = linePosition;
            Path = path;
            Value = value;
            ErrorType = errorType;
        }

        public static SchemaException FromValidationError(ValidationError validationError) {
            return new SchemaException(validationError.Message,
                                       validationError.LineNumber,
                                       validationError.LinePosition,
                                       validationError.Path,
                                       validationError.Value,
                                       validationError.ErrorType);
        }

    }

}