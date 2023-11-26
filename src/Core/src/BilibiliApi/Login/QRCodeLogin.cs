using QRCoder.Extensions;

using Core.Utils;
using Core.Logger;
using Core.Web;
using Core.BilibiliApi.Login.Model;
using QRCoder;
using System.Drawing;

namespace Core.BilibiliApi.Login {
    public class QrCodeLogin {

        public void LoginByQrCode(Action<QRCodeLoginResponse> callback) {
            AutoResetEvent getResult = new(false);
            QRCodeLoginResponse loginResult = new();

            ApplyForQRCode(tuple => {
                // * tuple.Item1: 登录用的网址
                ShowQrCode(tuple.Item1);
                // * tuple.Item2: 登录的密钥
                TryToLogin(tuple.Item2, getResult, loginResult);
            });
            getResult.WaitOne();
            if (loginResult == null || loginResult.GetQRCodeStatus() != QRCODE_SCAN_STATUS.SUCCESS) {
                LogManager.Info(nameof(LoginByQrCode), "Login by QR Code Failure.");
            } else {
                callback?.Invoke(loginResult!);
                // TODO 更新当前的状态
                
                LogManager.Info(nameof(LoginByQrCode), "Login by QR Code Success.");
            }
        }
        async public void ApplyForQRCode(Action<Tuple<string,string>> callback) {
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
        void ShowQrCode(string url) {
            // * 暂时用的，后续会用窗体展示二维码
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"AppData\QRCode\";
            string fileName = "img.png";
            if (!Directory.Exists(filePath)) {
                Directory.CreateDirectory(filePath);
            }
            File.WriteAllBytes(
                filePath + fileName, 
                PngByteQRCodeHelper.GetQRCode(url, QRCodeGenerator.ECCLevel.Q, 5));
        }

        void TryToLogin(
            string secreteKey,
            AutoResetEvent getResult,
            QRCodeLoginResponse loginResult
        ) {
            string url = @"https://passport.bilibili.com/x/passport-login/web/qrcode/poll";
            Dictionary<string, string> parameters = new(){
                {"qrcode_key", secreteKey}  
            };
            
            AutoResetEvent Pause = new(false);
            Task checkQRCodeScanResult = new(async obj => {
                while(true) {
                    var result = await WebClient.Request(url, "get", parameters);
                    if (result.Item1) {
                        var response = JsonUtils.ParseJsonString<QRCodeLoginResponse>(result.Item2);
                        if (response == null) {
                            LogManager.Error(nameof(JsonUtils.ParseJsonString), "Json Parse Failure");
                            continue;
                        }
                        if (response.GetShouldWait()) {
                            Pause.WaitOne(500, true);
                        } else {
                            loginResult = response;
                            break;
                        }
                    }
                }
                getResult.Set();
            }, null, TaskCreationOptions.LongRunning);

            checkQRCodeScanResult.Start();
            return;
        }
        
    }
}
