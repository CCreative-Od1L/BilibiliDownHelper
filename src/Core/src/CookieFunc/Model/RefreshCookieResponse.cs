using System.Text.Json.Serialization;

namespace Core.CookieFunc.Model;

public class RefreshCookieResponse {
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("ttl")]
    public int TTL { get; set; }
    [JsonPropertyName("data")]
    public RefreshCookieResponseData? Data { get; set; }

    public class RefreshCookieResponseData {
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }

    public string? GetRefreshToken() {
        return Data?.RefreshToken;
    }
}