using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Video.Model;

public class VideoDesc {
    [JsonPropertyName("raw_text")]
    public string? RawText { get; set; }
    /*
    * type=1时显示原文
    * type=2时显示'@'+raw_text+' '并链接至biz_id的主页
    */
    [JsonPropertyName("type")]
    public int Type { get; set; }
    [JsonPropertyName("biz_id")]
    public long BizId { get; set; }
    public string GetDesc() {
        if (Type == 1) { return RawText!; }
        else {
            return "@" + RawText! + " ";
        }
    }
}