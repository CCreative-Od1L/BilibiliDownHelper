using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

/// <summary>
/// * https://github.com/SocialSisterYi/bilibili-API-collect/blob/master/docs/misc/sign/wbi.md
/// * CSharp Part
/// </summary>
namespace Core.BilibiliApi.User;
public static class WbiAPI {
    private static readonly int[] MixinKeyEncTab = [
        46, 47, 18, 2, 53, 8, 23, 32, 15, 50, 10, 31, 58, 3, 45, 35, 27, 43, 5, 49, 33, 9, 42, 19, 29, 28, 14, 39,
        12, 38, 41, 13, 37, 48, 7, 16, 24, 55, 40, 61, 26, 17, 0, 1, 60, 51, 30, 4, 22, 25, 54, 21, 56, 59, 6, 63,
        57, 62, 11, 36, 20, 34, 44, 52
    ];
    
    //对 imgKey 和 subKey 进行字符顺序打乱编码
    private static string GetMixinKey(string orig) {
        return MixinKeyEncTab.Aggregate("", (s, i) => s + orig[i])[..32];
    }

    private static Dictionary<string, string> EncWbi(
        Dictionary<string, string> parameters,
        string imgKey,
        string subKey
    ) {
        string mixinKey = GetMixinKey(imgKey + subKey);
        string currTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        //添加 wts 字段
        parameters["wts"] = currTime;
        // 按照 key 重排参数
        parameters = parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
        //过滤 value 中的 "!'()*" 字符
        parameters = parameters.ToDictionary(
            kvp => kvp.Key,
            kvp => new string(kvp.Value.Where(chr => !"!'()*".Contains(chr)).ToArray())
        );
        // 序列化参数
        string query = new FormUrlEncodedContent(parameters).ReadAsStringAsync().Result;
        //计算 w_rid
        byte[] hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(query + mixinKey));
        string wbiSign = Convert.ToHexString(hashBytes).ToLower();
        parameters["w_rid"] = wbiSign;

        return parameters;
    }

    // 获取最新的 img_key 和 sub_key
    private static async Task<(string, string)> GetWbiKeys() {
        string WbiRequestUrl = "https://api.bilibili.com/x/web-interface/nav";
        var ( _, content) = await Web.WebClient.Request(url: WbiRequestUrl, methodName: "get");

        JsonNode response = JsonNode.Parse(content)!;

        string imgUrl = (string)response["data"]!["wbi_img"]!["img_url"]!;
        imgUrl = imgUrl.Split("/")[^1].Split(".")[0];

        string subUrl = (string)response["data"]!["wbi_img"]!["sub_url"]!;
        subUrl = subUrl.Split("/")[^1].Split(".")[0];
        return (imgUrl, subUrl);
    }
    /// <summary>
    /// * 获取已经添加了 wbi 信息的参数字符串，直接拼接在url后请求即可
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static async Task<string> GetAppendWbiUrl(Dictionary<string, string> parameters) {
        var (imgKey, subKey) = await GetWbiKeys();
        Dictionary<string, string> signedParams = EncWbi(
            parameters: parameters,
            imgKey: imgKey,
            subKey: subKey
        );
        return await new FormUrlEncodedContent(signedParams).ReadAsStringAsync();
        // * example: 'bar=514&baz=1919810&foo=114&wts=1687541921&w_rid=26e82b1b9b3a11dbb1807a9228a40d3b'
    }
}