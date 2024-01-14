using System.Text.Json.Serialization;

namespace Core.CookieFunc.Model;
public class CheckCookieResponse {
    [JsonPropertyName("code")]
    public int Code;
    [JsonPropertyName("message")]
    public string? Message;
    [JsonPropertyName("ttl")]
    public int TTL;
    [JsonPropertyName("data")]
    public CheckCookieResponseData? Data;

    public class CheckCookieResponseData() {
        [JsonPropertyName("refresh")]
        public bool Refresh;
        [JsonPropertyName("timestamp")]
        public long Timestamp;
    }
    public bool CheckIfRefresh() {
        if (Code == 0) {
            return Data!.Refresh;
        } else {
            // * 未登录
            return false;
        }
    }
}