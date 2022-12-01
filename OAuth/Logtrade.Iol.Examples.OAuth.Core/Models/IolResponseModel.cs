using System.Text.Json.Serialization;

namespace Logtrade.Iol.Examples.OAuth.Core.Models
{
    public class IolResponseModel
    {
        [JsonPropertyName("message_type")]
        public string MessageType { get; set; } = "";

        [JsonPropertyName("state")]
        public string State { get; set; } = "";

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = "";

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = "";

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = "";

        [JsonPropertyName("token_type")]
        public string AuthenticationType { get; set; } = "";

        [JsonPropertyName("connection_id")]
        public string ConnectionID { get; set; } = "";

        [JsonPropertyName("accepting_party")]
        public Dictionary<string, string> AcceptingParty { get; set; } = new Dictionary<string, string>();
    }
}
