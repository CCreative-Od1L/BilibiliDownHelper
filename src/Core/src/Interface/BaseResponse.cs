using System.Text.Json.Serialization;

namespace Core.PleInterface;

public class BaseResponse<T> {
    [JsonPropertyName("code")]
    public int Code { get; set; } = -1;
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("ttl")]
    public int TTL { get; set; }
    [JsonPropertyName("data")]
    public T? Data { get; set; }
    public virtual bool IsValid() {
        return Code >= 0;
    }
}