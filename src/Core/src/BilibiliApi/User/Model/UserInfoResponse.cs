using System.Text.Json.Serialization;

namespace Core.BilibiliApi.User;

public class UserInfoResponse {
    [JsonPropertyName("code")]
    public int Code = -1;
    [JsonPropertyName("message")]
    public string? Message;
    [JsonPropertyName("ttl")]
    public int TTL;
    [JsonPropertyName("data")]
    public UserInfo? userInfo;
    public bool CheckIsValid() {
        return Code.Equals(0);
    }
}