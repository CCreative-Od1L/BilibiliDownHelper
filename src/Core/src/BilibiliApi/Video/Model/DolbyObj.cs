using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Video.Model;

public class DolbyObj {
    [JsonPropertyName("type")]
    public int Type { get; set; }
    [JsonPropertyName("audio")]
    public VideoAndAudioObj[] Audio { get; set; } = [];
}