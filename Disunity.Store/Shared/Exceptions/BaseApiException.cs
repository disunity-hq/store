using System;

using Newtonsoft.Json;


namespace Disunity.Store.Exceptions {

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseApiException : Exception {

        [JsonProperty] public string Name { get; }
        [JsonProperty] public string Context { get; }
        [JsonProperty] public override string Message => base.Message;

        protected BaseApiException(string message, string name = null, string context = null) : base(message) {
            Name = name ?? GetType().Name;
            Context = context ?? TargetSite?.Name ?? "Unknown";
        }

        public override string ToString() {
            return $"{Name} @{Context}: {Message}";
        }

    }

}