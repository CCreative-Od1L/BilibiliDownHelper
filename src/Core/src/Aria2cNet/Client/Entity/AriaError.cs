using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;

//"error": {
//    "code": 1,
//    "message": "Unauthorized"
//}
public class AriaError
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = "";

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}