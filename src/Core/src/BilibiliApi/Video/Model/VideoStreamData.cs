using System.Text.Json.Serialization;
using Core.PleInterface;

namespace Core.BilibiliApi.Video.Model;
public class VideoStreamResponse :BaseResponse<VideoStreamData> {
    public override bool IsValid() {
        return base.IsValid();
    }
}

public class VideoStreamData {
    [JsonPropertyName("quality")]
    public int Quality { get; set; }
    [JsonPropertyName("timelength")]
    public int TimeLength { get; set; }
    [JsonPropertyName("accept_description")]    // * 支持的清晰度名称列表
    public string[] AcceptDescription { get; set; } = [];
    [JsonPropertyName("accept_quality")]        // * 支持的清晰度代号列表
    public int[] AcceptQuality { get; set; } = [];
    [JsonPropertyName("video_codecid")]
    public int VideoCodeCid { get; set; }
    [JsonPropertyName("durl")]              // * MP4 存在此字段
    public string[] Durl { get; set; } = [];
    [JsonPropertyName("dash")]              // * DASH 存在此字段
    public DashVideoObj? Dash { get; set; } = null;
    public VIDEO_QUALITY GetVideoQuality() {
        return (VIDEO_QUALITY)Quality;
    }
    public VIDEO_CODE_CID GetVideoCodeCid() {
        return (VIDEO_CODE_CID)VideoCodeCid;
    }
}