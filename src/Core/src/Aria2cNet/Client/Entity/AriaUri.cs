using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
public class AriaUri
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}