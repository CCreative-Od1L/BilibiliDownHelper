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
    public long VideoCodeCid { get; set; }
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
    /// <summary>
    /// 处理获取视频下载链接(DASH)
    /// </summary>
    /// <returns></returns>
    public (List<List<string>>, List<VIDEO_QUALITY>) GetDashVideoDownloadLink() {
        if (DashOrMp4() != 1) { return ([],[]); }
        return Dash!.GetVideoDownloadLink();
    }
    /// <summary>
    /// 处理获取音频下载链接(DASH)
    /// </summary>
    /// <returns></returns>
    public  (List<List<string>>, List<AUDIO_QUALITY>)GetDashAudioDownloadLink() {
        if (DashOrMp4() != 1) { return ([],[]); }
        return Dash!.GetAudioDownloadLink();
    }
    /// <summary>
    /// * 1 : Dash
    /// * 2 : MP4
    /// * 0 : none 
    /// </summary>
    /// <returns></returns>
    public int DashOrMp4() {
        return (Dash == null) ? ((Durl.Length == 0)? 2 : 0) : 1;
    }
}