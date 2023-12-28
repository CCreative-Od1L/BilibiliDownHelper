using System.Text.Json.Serialization;
using Core.Utils;
namespace Core.Aria2cNet.Client.Entity;
public class AriaGetOption {
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }
    [JsonPropertyName("result")]
    public AriaOption? Result { get; set; }
    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }
    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}