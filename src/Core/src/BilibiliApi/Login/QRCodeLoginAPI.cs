using Core.Utils;
using Core.Web;
using Core.BilibiliApi.Login.Model;
using QRCoder;
using Core.BilibiliApi.User;

namespace Core.BilibiliApi.Login {
    public static class QrCodeLoginAPI {
        /// <summary>
        /// * 使用QR码登录 API
        /// </summary>
        static public async void LoginByQrCode(Action<byte[]>? qrcodeLoadCallback) {
            AutoResetEvent getResult = new(false);
            QRCodeLoginResponse loginResult = new();

            ApplyForQRCode(tuple => {
                // * tuple.Item1: 登录用的网址
                ShowQrCode(tuple.Item1, qrcodeLoadCallback);
                // * tuple.Item2: 登录的密钥
                TryToLogin(tuple.Item2, getResult, (QRCodeLoginResponse response) => {
                    loginResult = response;
                });
            });
            getResult.WaitOne();
            if (loginResult == null || loginResult.GetQRCodeStatus() != QRCODE_SCAN_STATUS.SUCCESS) {
                CoreManager.logger.Info(nameof(LoginByQrCode), "Login by QR Code Failure.");
            } else {
                await UserInfoAPI.INSTANCE.UpdateMyInfoAsync(loginResult.Data!);
                CoreManager.logger.Info(nameof(LoginByQrCode), "Login by QR Code Success.");
            }
        }
        /// <summary>
        /// * 申请二维码
        /// </summary>
        /// <param name="callback">成功获取二维码信息回调</param>
        static async public void ApplyForQRCode(Action<Tuple<string,string>> callback) {
            string url = @"https://passport.bilibili.com/x/passport-login/web/qrcode/generate";
            var (isSuccess, content) = await WebClient.Request(
                url: url,
                methodName: "get");
            if (isSuccess) {
                var parseRes = JsonUtils.ParseJsonString<ApplyQRCodeData>(content);
                if (parseRes != null && parseRes.Data != null && parseRes.Data.IsValid()) {
                    callback?.Invoke(new(parseRes.Data.Url, parseRes.Data.QRCodeKey));
                    return;
                } else {
                    CoreManager.logger.Error(nameof(JsonUtils.ParseJsonString), "Json Parse Failure");
                }
            } else {
                CoreManager.logger.Error(nameof(ApplyForQRCode), "Apply for QRCode Error");
            }
            return;
        }
        /// <summary>
        /// * 展示QR码
        /// </summary>
        /// <param name="url"></param>
        static void ShowQrCode(string url, Action<byte[]>? loadCallback) {
            // * debug用，后续会用窗体展示二维码
            //string filePath = AppDomain.CurrentDomain.BaseDirectory + @"AppData\QRCode\";
            //string fileName = "img.png";
            //if (!Directory.Exists(filePath)) {
            //    Directory.CreateDirectory(filePath);
            //}
            //File.WriteAllBytes(
            //    filePath + fileName, 
            //    PngByteQRCodeHelper.GetQRCode(url, QRCodeGenerator.ECCLevel.Q, 5));
            loadCallback?.Invoke(PngByteQRCodeHelper.GetQRCode(url, QRCodeGenerator.ECCLevel.Q, 5));
        }
        /// <summary>
        /// * 尝试登录
        /// </summary>
        /// <param name="secreteKey"></param>
        /// <param name="getResult"></param>
        /// <param name="loginResult"></param>
        static void TryToLogin(
            string secreteKey,
            AutoResetEvent getResult,
            Action<QRCodeLoginResponse>? callback
        ) {
            string url = @"https://passport.bilibili.com/x/passport-login/web/qrcode/poll";
            Dictionary<string, string> parameters = new(){
                {"qrcode_key", secreteKey}  
            };
            
            AutoResetEvent Pause = new(false);
            Task checkQRCodeScanResult = new(async obj => {
                while(true) {
                    var result = await WebClient.Request(
                        url: url,
                        methodName: "get",
                        parameters: parameters);
                    if (result.Item1) {
                        var response = JsonUtils.ParseJsonString<QRCodeLoginResponse>(result.Item2);
                        if (response == null) {
                            CoreManager.logger.Error(nameof(JsonUtils.ParseJsonString), "Json Parse Failure");
                            continue;
                        }
                        if (response.GetShouldWait()) {
                            Pause.WaitOne(500, true);
                        } else {
                            callback?.Invoke(response);
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
