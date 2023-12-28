using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
public class AriaSendData {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }

    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("params")]
    public List<object>? Params { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaSendOption {
    [JsonPropertyName("all-proxy")]
    public string? HttpProxy { get; set; }

    [JsonPropertyName("out")]
    public string? Out { get; set; }

    [JsonPropertyName("dir")]
    public string? Dir { get; set; }

    [JsonPropertyName("user-agent")]
    public string? UserAgent { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}