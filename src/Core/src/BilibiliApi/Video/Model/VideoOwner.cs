using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Video.Model;
public class VideoOwner {
    [JsonPropertyName("mid")]
    public long Mid { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("face")]
    public string? Face { get; set; }
}