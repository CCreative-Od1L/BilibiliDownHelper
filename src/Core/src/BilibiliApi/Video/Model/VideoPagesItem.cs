using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Video.Model;
public class VideoPagesItem {
    [JsonPropertyName("cid")]       // * 分P cid
    public long Cid { get; set; }
    [JsonPropertyName("page")]      // * 分P序号
    public int Page { get; set; }
    [JsonPropertyName("part")]      // * 分P标题
    public string Part { get; set; } = string.Empty;
    [JsonPropertyName("duration")]
    public int Duration { get; set; }   // * 持续时间
}