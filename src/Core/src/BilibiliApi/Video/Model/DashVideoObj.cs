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
    public bool IsDolby() { return Dolby == null; }
    public bool IsFlac() { return Flac == null; }
    public (List<List<string>>, List<VIDEO_QUALITY>) GetVideoDownloadLink() {
        List<List<string>> resourceLink = [];
        List<VIDEO_QUALITY> qnList = [];
        for(int i = 0; i < Video.Length; ++i) {
            resourceLink.Add([
                Video[i].GetBaseUrl(),
                ..Video[i].GetBackupUrl(),
            ]);
            qnList.Add((VIDEO_QUALITY)Video[i].ID);
        }
        return (resourceLink, qnList);
    }
    public (List<List<string>>, List<AUDIO_QUALITY>) GetAudioDownloadLink() {
        List<List<string>> resourceLink = [];
        List<AUDIO_QUALITY> qnList = [];
        for(int i = 0; i < Audio.Length; ++i) {
            resourceLink.Add([
                Audio[i].GetBaseUrl(),
                ..Audio[i].GetBackupUrl(),
                ]);
            qnList.Add((AUDIO_QUALITY)Audio[i].ID);
        }
        return (resourceLink, qnList);
    }
}