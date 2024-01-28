using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Video.Model;

public class VideoAndAudioObj {
    [JsonPropertyName("id")]
    public int ID { get; set; }
    [JsonPropertyName("baseUrl")]
    public string BaseUrl1 { get; set; } = "";
    [JsonPropertyName("base_url")]
    public string BaseUrl2 { get; set; } = "";
    [JsonPropertyName("backupUrl")]
    public string[] BackupUrl1 { get; set; } = [];
    [JsonPropertyName("backup_url")]
    public string[] BackupUrl2 { get; set; } = [];
    [JsonPropertyName("codecs")]            // * 编码/音频类型
    public string Codecs { get; set; } = "";
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
    [JsonPropertyName("frameRate")]
    public string FrameRate1 { get; set; } = "";
    [JsonPropertyName("frame_rate")]
    public string FrameRate2 { get; set; } = "";
    [JsonPropertyName("SegmentBase")]
    public SegmentBaseObj? SegmentBase1 { get; set; } = null;
    [JsonPropertyName("segment_base")]
    public SegmentBaseObj? SegmentBase2 { get; set; } = null;
    [JsonPropertyName("codecid")]       // * 码流编码标识代码
    public int CodeCid { get; set; }
    public string GetBaseUrl() {
        return BaseUrl1 ?? BaseUrl2;
    }
    public List<string> GetBackupUrl() {
        return (BackupUrl1.Length != 0) ? [..BackupUrl1] : [..BackupUrl2];
    }
    public string GetFrameRate() {
        return FrameRate1 ?? FrameRate2;
    }
}