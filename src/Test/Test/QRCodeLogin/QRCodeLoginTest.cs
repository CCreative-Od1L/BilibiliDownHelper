using Core.Web;
using Core.BilibiliApi.Login;
using Core.BilibiliApi.Login.Model;
using QRCoder;
using Xunit.Abstractions;
using Core.Utils;
using Core.Logger;

namespace Core.Test {
    public class QrCodeLoginTest {
        readonly ITestOutputHelper output;
        public QrCodeLoginTest(ITestOutputHelper testOutputHelper) {
            output = testOutputHelper;
        }

        AutoResetEvent getResult = new(false);
        string loginResult = string.Empty;
        
        [Fact]
        public void IsWorkWell() {
            // QrCodeLoginAPI.LoginByQrCode();
            
            // QrCodeLogin.ApplyForQRCode(tuple => {
            //     // * tuple.Item1: 登录用的网址
            //     ShowQrCode(tuple.Item1);
            //     // * tuple.Item2: 登录的密钥
            //     TryToLogin(tuple.Item2);
            // });
            // getResult.WaitOne();
            // output.WriteLine(loginResult);
        }

        static void ShowQrCode(string url) {
            // * 暂时用的，后续会用窗体展示
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"AppData\QRCode\";
            string fileName = "img.png";
            if (!Directory.Exists(filePath)) {
                Directory.CreateDirectory(filePath);
            }
            File.WriteAllBytes(
                filePath + fileName, 
                PngByteQRCodeHelper.GetQRCode(url, QRCodeGenerator.ECCLevel.Q, 5));
        }

        void TryToLogin(string secreteKey) {
            string url = @"https://passport.bilibili.com/x/passport-login/web/qrcode/poll";
            Dictionary<string, string> parameters = new(){
                {"qrcode_key", secreteKey}  
            };
            
            AutoResetEvent Pause = new(false);
            Task checkQRCodeScanResult = new(async obj => {
                while(true) {
                    var result = await WebClient.RequestJson(url: url, methodName: "get", parameters: parameters);
                    if (result.Item1) {
                        var response = JsonUtils.ParseJsonString<QRCodeLoginResponse>(result.Item2);
                        if (response == null) {
                            CoreManager.logger.Error(nameof(JsonUtils.ParseJsonString), "Json Parse Failure");
                            continue;
                        }
                        if (response.GetShouldWait()) {
                            Pause.WaitOne(500, true);
                        } else {
                            loginResult = result.Item2;
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

