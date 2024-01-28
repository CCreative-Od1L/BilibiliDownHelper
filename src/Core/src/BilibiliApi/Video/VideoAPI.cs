using System.Text.RegularExpressions;
using Core.BilibiliApi.Video.Model;
using Core.Utils;

namespace Core.BilibiliApi.Video;
public static class VideoAPI {
    /// <summary>
    /// * 获取视频的基本信息
    /// </summary>
    /// <param name="avid"></param>
    /// <param name="bvid"></param>
    /// <returns></returns>
    public static async Task<(bool, VideoBaseInfoData?)> GetVideoBaseInfoFromID(string avid, string bvid) {
        if(string.IsNullOrEmpty(avid) && string.IsNullOrEmpty(bvid)) { return (false, null); }

        string videoInfoRequestUrl = "https://api.bilibili.com/x/web-interface/view";
        var (isSuccess, content) = await Web.WebClient.Request(
            url: videoInfoRequestUrl,
            methodName: "get",
            parameters: new Dictionary<string, string> {
                { "aid", avid },
                { "bvid", bvid }
            }
        );
        if (!isSuccess) { return (false, null); }
        else {
            return (true, JsonUtils.ParseJsonString<VideoBaseInfoResponse>(content)!.Data);
        } 
    }
    /// <summary>
    /// * 获取视频流信息
    /// </summary>
    /// <param name="cid"></param>
    /// <param name="avid"></param>
    /// <param name="bvid"></param>
    /// <param name="fnval"></param>
    /// <param name="fourk"></param>
    /// <returns></returns>
    public static async Task<(bool, VideoStreamData?)> GetVideoStreamDataFromID(
        string cid,
        string avid = "",
        string bvid = "",
        int fnval = 16,
        int fourk = 0
    ) {
        if(string.IsNullOrEmpty(avid) && string.IsNullOrEmpty(bvid)) { return (false, null); }
        // * new Url
        //string videoStreamRequestUrl = "https://api.bilibili.com/x/player/wbi/playurl";
        string videoStreamRequestUrl = "https://api.bilibili.com/x/player/playurl";
        var (isSuccess, content) = await Web.WebClient.Request(
            url: videoStreamRequestUrl,
            methodName: "get",
            //useWbi: true,
            parameters: new Dictionary<string, string> {
                { "avid", avid },
                { "bvid", bvid },
                { "cid", cid },
                { "qn", "0" },
                { "fnval", fnval.ToString() },
                { "fourk", fourk.ToString() }
            }
        );
        if (!isSuccess) { return (false, null); }
        else {
            return (true, JsonUtils.ParseJsonString<VideoStreamResponse>(content)!.Data);
        }
    }
    /// <summary>
    /// * 解析输入
    /// </summary>
    /// <param name="val"></param>
    /// <returns>(isSuccess, avid, bvid)</returns>
    public static (bool, string, string) ParseInputStr(string val) {
        string urlReg = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        string bvidReg = @"^((b|B)(v|V))";

        bool isUrl = Regex.IsMatch(val, urlReg);
        if (isUrl) {
            var (isSuccess, avid, bvid_raw) = ParseBilibiliVideoUrl(val);
            if (isSuccess && Regex.IsMatch(bvid_raw, bvidReg)) {
                return (true, avid, bvid_raw[2..]);
            }
            return (false, string.Empty, string.Empty);
        }

        string avidReg = @"^((a|A)(v|V))";
        if (Regex.IsMatch(val, bvidReg)) {
            return (true, "", val[2..]);
        }
        if (Regex.IsMatch(val, avidReg)) {
            return (true, val[2..], "");
        }
        return (false, string.Empty, string.Empty);
    }
    /// <summary>
    /// * 解析Bilibili视频Urlz
    /// </summary>
    /// <param name="url"></param>
    /// <returns>(isSuccess, avid, bvid)</returns>
    public static (bool, string, string) ParseBilibiliVideoUrl(string url) {
        // * 目前网址已经全面启用BV号，所以Av号位置固定为空
        string format = @"http(s)?://www.bilibili.com/video/([\w]+)/";
        var regexResult = Regex.Match(url, format).Groups;
        if (regexResult.Count == 0) {
            return (false, string.Empty, string.Empty);
        } else {
            return (true, string.Empty, regexResult[^1].Value);
        }
    }
}