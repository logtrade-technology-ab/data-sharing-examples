using System.Text.Json.Serialization;

namespace Logtrade.Iol.Examples.OAuth.Core.Models
{
    public class BearerTokenResponseModel
    {
        [JsonPropertyName("access_token")]
        public string BearerToken { get; set; } = "";

        [JsonPropertyName("accepting_party")]
        public Dictionary<string, string> AcceptingParty { get; set; } = new Dictionary<string, string>();
    }
}
