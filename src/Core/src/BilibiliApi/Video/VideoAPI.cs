using Core.BilibiliApi.Video.Model;
using Core.Utils;

namespace Core.BilibiliApi.Video;
public static class VideoAPI {
    public static async Task<(bool, VideoBaseInfoResponse?)> GetVideoBaseInfoFromID(string avid, string bvid) {
        if(string.IsNullOrEmpty(avid) && string.IsNullOrEmpty(bvid)) { return (false, null); }

        string VideoInfoRequestUrl = "https://api.bilibili.com/x/web-interface/view";
        var (isSuccess, content) = await Web.WebClient.Request(
            url: VideoInfoRequestUrl,
            methodName: "get",
            new Dictionary<string, string> {
                { "aid", avid },
                { "bvid", bvid }
            }
        );
        if (!isSuccess) { return (false, null); }
        else {
            return (true, JsonUtils.ParseJsonString<VideoBaseInfoResponse>(content));
        } 
    }
}