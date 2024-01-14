using System.Text.Json.Serialization;

namespace Core.CookieFunc.Model;
public class CheckCookieResponse {
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("ttl")]
    public int TTL { get; set; }
    [JsonPropertyName("data")]
    public CheckCookieResponseData? Data { get; set; }

    public class CheckCookieResponseData() {
        [JsonPropertyName("refresh")]
        public bool Refresh { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
    public bool CheckIfRefresh() {
        if (Code == 0) {
            return Data!.Refresh;
        } else {
            // * 未登录
            return false;
        }
    }
    public long GetTimestamp() {
        return Data!.Timestamp;
    }
}