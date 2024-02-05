using Core.Utils;
using Core.Web;
using Core.BilibiliApi.Login.Model;
using QRCoder;
using Core.BilibiliApi.User;
using System;
using System.Drawing;

namespace Core.BilibiliApi.Login {
    public static class QrCodeLoginAPI {
        /// <summary>
        /// * 使用QR码登录 API
        /// </summary>
        static public async void LoginByQrCode(
            Action<byte[]>? qrcodeLoadCallback,
            Action? qrcodeScanCallback) {
            var (url, qrCodeKey) = await ApplyForQRCode();
            ShowQrCode(url, qrcodeLoadCallback);
            TryToLogin(
                qrCodeKey,
                qrcodeScanCallback,
                (QRCodeLoginResponse loginResult) => {
                    if (loginResult == null || loginResult.GetQRCodeStatus() != QRCODE_SCAN_STATUS.SUCCESS) {
                        CoreManager.logger.Info(nameof(LoginByQrCode), "Login by QR Code Failure.");
                    } else {
                        _ = UserInfoAPI.INSTANCE.UpdateMyInfoAsync(loginResult.Data!);
                        CoreManager.logger.Info(nameof(LoginByQrCode), "Login by QR Code Success.");
                    }
                });
        }
        /// <summary>
        /// * 申请二维码
        /// </summary>
        /// <param name="callback">成功获取二维码信息回调</param>
        static async public Task<(string, string)> ApplyForQRCode() {
            string url = @"https://passport.bilibili.com/x/passport-login/web/qrcode/generate";
            var (isSuccess, content) = await WebClient.Request(
                url: url,
                methodName: "get");
            if (isSuccess) {
                var parseRes = JsonUtils.ParseJsonString<ApplyQRCodeData>(content);
                if (parseRes != null && parseRes.Data != null && parseRes.Data.IsValid()) {
                    return (parseRes.Data.Url, parseRes.Data.QRCodeKey);
                } else {
                    CoreManager.logger.Error(nameof(JsonUtils.ParseJsonString), "Json Parse Failure");
                }
            } else {
                CoreManager.logger.Error(nameof(ApplyForQRCode), "Apply for QRCode Error");
            }
            return (string.Empty, string.Empty);
        }
        /// <summary>
        /// * 展示QR码
        /// </summary>
        /// <param name="url"></param>
        static void ShowQrCode(string url, Action<byte[]>? loadCallback) {
            loadCallback?.Invoke(
                BitmapByteQRCodeHelper.GetQRCode(url, QRCodeGenerator.ECCLevel.Q, 5));
        }
        /// <summary>
        /// * 尝试登录
        /// </summary>
        /// <param name="secreteKey"></param>
        /// <param name="getResult"></param>
        /// <param name="loginResult"></param>
        static void TryToLogin(
            string secreteKey,
            Action? qrcodeScanCallback,
            Action<QRCodeLoginResponse>? resultCallback
        ) {
            string url = @"https://passport.bilibili.com/x/passport-login/web/qrcode/poll";
            Dictionary<string, string> parameters = new(){
                {"qrcode_key", secreteKey}  
            };
            
            AutoResetEvent Pause = new(false);
            Task checkQRCodeScanResult = new(async obj => {
                var startTime = long.Parse(DateTimeUtils.GetCurrentTimestampSecond());
                while(true) {
                    var (isSuccess, content) = await WebClient.Request(
                        url: url,
                        methodName: "get",
                        parameters: parameters);
                    if (isSuccess) {
                        var response = JsonUtils.ParseJsonString<QRCodeLoginResponse>(content);
                        if (response == null) {
                            CoreManager.logger.Error(nameof(JsonUtils.ParseJsonString), "Json Parse Failure");
                            continue;
                        }
                        if (response.GetShouldWait()) {
                            if(response.GetHasScaned()) {
                                qrcodeScanCallback?.Invoke();
                            }
                            Pause.WaitOne(500, true);
                        } else {
                            resultCallback?.Invoke(response);
                            break;
                        }
                        
                    }
                    // * 60秒超时-自动退出
                    if(long.Parse(DateTimeUtils.GetCurrentTimestampSecond()) - startTime > 60) {
                        CoreManager.logger.Info("QRcode登录超时,需刷新二维码");
                        break;
                    }
                }
            }, null, TaskCreationOptions.LongRunning);

            checkQRCodeScanResult.Start();
            return;
        }
    }
}
