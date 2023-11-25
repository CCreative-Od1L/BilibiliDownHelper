using QRCoder.Extensions;

using Core.Utils;
using Core.Logger;
using Core.Web;
using Core.BilibiliApi.Login.Model;
using QRCoder;
using System.Drawing;

namespace Core.BilibiliApi.Login {
    public class QrCodeLogin {
        static async public void ApplyForQRCode(Action<Tuple<string,string>> callback) {
            string url = @"https://passport.bilibili.com/x/passport-login/web/qrcode/generate";
            var result = await WebClient.Request(url, "get");
            if (result.Item1) {
                var parseRes = JsonUtils.ParseJsonString<ApplyQRCodeData>(result.Item2);
                if (parseRes != null) {
                    callback?.Invoke(new Tuple<string, string>(parseRes.data.url, parseRes.data.qrcode_key));
                    return;
                } else {
                    LogManager.Error(nameof(JsonUtils.ParseJsonString), "Json Parse Failure");
                }
            } else {
                LogManager.Error(nameof(ApplyForQRCode), "Apply for QRCode Error");
            }
            return;
        }

        static public void LoginByQrCode() {
            ApplyForQRCode(tuple => {
                // * tuple.Item1: 登录用的网址
                ShowQrCode(tuple.Item1);
                // * tuple.Item2: 登录的密钥
                TryToLogin(tuple.Item2);
            });
            LogManager.Info(nameof(LoginByQrCode), "Login by QR Code");
        }

        static void ShowQrCode(string url) {

        }

        static void TryToLogin(string secreteKey) {

        }
        
    }
}
