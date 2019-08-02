using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Disunity.Store.Errors {

    public class MissingArtifactError : ApiError {
        
        [JsonProperty]
        public string Filename { get; }

        public MissingArtifactError(string filename)
            : base($"Manifest mentions artifact not present in archive") {
            Filename = filename;
        }

    }

}