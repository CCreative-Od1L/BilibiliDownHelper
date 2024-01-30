using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
public class AriaGetFiles {
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }
    [JsonPropertyName("result")]
    public List<AriaGetFilesResult>? Result { get; set; }
    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }
    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaGetFilesResult
{
    [JsonPropertyName("completedLength")]
    public string? CompletedLength { get; set; }

    [JsonPropertyName("index")]
    public string? Index { get; set; }

    [JsonPropertyName("length")]
    public string? Length { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("selected")]
    public string? Selected { get; set; }
    
    [JsonPropertyName("uris")]
    public List<AriaUri>? Uris { get; set; }
    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}