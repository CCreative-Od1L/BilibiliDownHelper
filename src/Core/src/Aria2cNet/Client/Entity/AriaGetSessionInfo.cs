using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
public class AriaGetSessionInfo {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }

    [JsonPropertyName("result")]
    public AriaGetSessionInfoResult? Result { get; set; }

    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }

    public override string? ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaGetSessionInfoResult {
    [JsonPropertyName("sessionId")]
    public string? SessionId { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
