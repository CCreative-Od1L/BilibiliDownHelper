using Gma.QrCodeNet.Encoding;

using Core.Utils;
using Core.Logger;
using Core.Web;
using Core.BilibiliApi.Login.Model;

namespace Core.BilibiliApi.Login {
    public class QrCodeLogin {
        static public async Task<Tuple<string, string>> ApplyForQRCode() {
            string url = @"https://passport.bilibili.com/x/passport-login/web/qrcode/generate";
            var result = await WebClient.Request(url, "get");
            if (result.Item1) {
                var parseRes = JsonUtils.ParseJsonString<ApplyQRCodeData>(result.Item2);
                if (parseRes != null) {
                    return new Tuple<string, string>(parseRes.data.url, parseRes.data.qrcode_key);
                } else {
                    LogManager.Error(nameof(JsonUtils.ParseJsonString), "Json Parse Failure");
                    return new Tuple<string, string>("", "");
                }
            } else {
                LogManager.Error(nameof(ApplyForQRCode), "Apply for QRCode Error");
                return new Tuple<string, string>("", "");
            }
        }
    }
}
