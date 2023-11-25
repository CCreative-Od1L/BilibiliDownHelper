using System.Diagnostics;
using System.Drawing;
using Core.BilibiliApi.Login;
using QRCoder;
using Xunit.Abstractions;
namespace Test;

public class QrCodeLoginTest {
    readonly ITestOutputHelper output;

    public QrCodeLoginTest(ITestOutputHelper testOutputHelper) {
        output = testOutputHelper;
    }

    [Fact]
    public void IsWorkWell() {
        QrCodeLogin.ApplyForQRCode(tuple => {
                // * tuple.Item1: 登录用的网址
                ShowQrCode(tuple.Item1);
                // * tuple.Item2: 登录的密钥
                TryToLogin(tuple.Item2);
        });
        Thread.Sleep(2000);
    }

    void ShowQrCode(string url) {
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
        return;
    }
}