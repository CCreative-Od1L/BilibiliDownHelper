using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Video.Model;
public class DashVideoObj {
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    [JsonPropertyName("video")]     // * 视频流下载链接列表
    public VideoAndAudioObj[] Video { get; set; } = [];
    [JsonPropertyName("audio")]     // * 音频流下载链接列表
    public VideoAndAudioObj[] Audio { get; set; } = [];
    [JsonPropertyName("dolby")]
    public DolbyObj? Dolby { get; set; } = null;
    [JsonPropertyName("flac")]
    public FlacObj? Flac { get; set; } = null;
}