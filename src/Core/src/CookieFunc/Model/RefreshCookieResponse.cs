using System.Text.Json.Serialization;
using Core.PleInterface;

namespace Core.CookieFunc.Model;

public class RefreshCookieResponse : BaseResponse<RefreshCookieResponseData>{
    public string? GetRefreshToken() {
        return Data?.RefreshToken;
    }
}
public class RefreshCookieResponseData {
    [JsonPropertyName("status")]
    public int Status { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
}