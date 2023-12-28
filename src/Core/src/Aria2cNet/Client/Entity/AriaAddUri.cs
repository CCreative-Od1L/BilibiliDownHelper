using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
// * json message example
/*
{
    "id": "downkyi",
    "jsonrpc": "2.0",
    "result": "1aac102a4875c8cd"  // * guid
}
*/
public class AriaAddUri {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }

    [JsonPropertyName("result")]
    public string? Result { get; set; }
    
    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }
    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
