﻿using System.Text.Json.Serialization;

namespace Logtrade.Iol.Examples.OAuth.Core.Models.Iol;

public class BearerTokenResponseModel
{
    [JsonPropertyName("access_token")]
    public string BearerToken { get; set; } = "";
}
