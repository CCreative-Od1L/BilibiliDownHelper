using System.Text;
using System.Text.Json.Serialization;
using Core.PleInterface;

namespace Core.BilibiliApi.Video.Model;
public class VideoBaseInfoResponse : BaseResponse<VideoBaseInfoData> {
    public override bool IsValid() {
        return base.IsValid();
    }
}
public class VideoBaseInfoData {
    [JsonPropertyName("bvid")] 
    public string Bvid { get; set; } = "";
    [JsonPropertyName("aid")]
    public long Avid { get; set; }
    [JsonPropertyName("videos")]
    public int Videos { get; set; }
    [JsonPropertyName("pic")]
    public string CoverUrl { get; set; } = "";
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";
    [JsonPropertyName("desc_v2")]   // * 新版视频简介
    public VideoDesc[] DescV2 { get; set; } = [];
    [JsonPropertyName("owner")]
    public VideoOwner Owner { get; set; } = new();
    [JsonPropertyName("pages")]
    public VideoPagesItem[] Pages { get; set; } = [];
    public string GetVideoDesc() {
        if (DescV2 == null) { return string.Empty; }
        StringBuilder descBuilder  = new();
        for(int i = 0; i < DescV2.Length; ++i) {
            descBuilder.AppendLine(DescV2[i].GetDesc());
        }
        return descBuilder.ToString();
    }
    public List<long> GetPagesCid() {
        List<long> li = [];
        for(int i = 0; i < Pages.Length; i++) {
            li.Add(Pages[i].Cid);
        }
        return li;
    }
}
