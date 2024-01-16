using System.Text.Json.Serialization;
using Core.PleInterface;
using Core.Utils;

namespace Core.CookieFunc.Model;
public class CheckCookieResponse : BaseResponse<CheckCookieResponseData> {
    public bool CheckIfRefresh() {
        return (Code == 0) && Data!.Refresh;
    }
    public long GetTimestamp() {
        if (Data == null) { return 0; }
        else return Data!.Timestamp;
    }
}
public class CheckCookieResponseData() {
    [JsonPropertyName("refresh")]
    public bool Refresh { get; set; }
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
}