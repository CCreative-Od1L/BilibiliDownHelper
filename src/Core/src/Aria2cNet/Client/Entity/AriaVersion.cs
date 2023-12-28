using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
public class AriaVersion {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }

    [JsonPropertyName("result")]
    public AriaVersionResult? Result { get; set; }

    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaVersionResult {
    [JsonPropertyName("enabledFeatures")]
    public List<string>? EnabledFeatures { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
