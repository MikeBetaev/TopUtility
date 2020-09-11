using System.Text.Json.Serialization;

namespace Top
{
    public class ClientRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public RequestedClientActionEnum Action { get; set; }
    }
}
