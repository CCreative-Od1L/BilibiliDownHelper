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
    [JsonPropertyName("bvid")]              // * BV
    public string Bvid { get; set; } = "";
    [JsonPropertyName("aid")]               // * AV
    public long Avid { get; set; }
    [JsonPropertyName("videos")]            // * 分P总数
    public int Videos { get; set; }
    [JsonPropertyName("pic")]               // * 封面图URL
    public string CoverUrl { get; set; } = "";
    [JsonPropertyName("title")]             // * 标题
    public string Title { get; set; } = "";
    [JsonPropertyName("desc_v2")]           // * 新版视频简介
    public VideoDesc[] DescV2 { get; set; } = [];
    [JsonPropertyName("owner")]             // * 发布者信息
    public VideoOwner Owner { get; set; } = new();
    [JsonPropertyName("pages")]             // * 分P cids
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
    public List<(long, int, string, int)> GetPagesInfo() {
        List<(long, int, string, int)> li = [];
        for(int i = 0; i < Pages.Length; ++i) {
            li.Add((Pages[i].Cid, Pages[i].Page, Pages[i].Part, Pages[i].Duration));
        }
        return li;
    }
}
