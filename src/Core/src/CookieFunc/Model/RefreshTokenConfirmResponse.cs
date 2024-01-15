using System.Text.Json.Serialization;

namespace Core.CookieFunc.Model;

public class RefreshTokenConfirmResponse {
    [JsonPropertyName("code")]
    public int Code { get; set; } = -1;
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("ttl")]
    public int TTL { get; set; }

    public bool CheckIsValid() {
        return Code >= 0;
    }
}