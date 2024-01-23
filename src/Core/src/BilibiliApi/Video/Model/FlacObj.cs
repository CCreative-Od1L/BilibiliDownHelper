using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Video.Model;

public class FlacObj {
    [JsonPropertyName("display")]
    public bool Display { get; set; }
    [JsonPropertyName("audio")]
    public VideoAndAudioObj? Audio { get; set; } = null;
}