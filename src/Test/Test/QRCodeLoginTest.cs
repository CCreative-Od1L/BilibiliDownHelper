using Core.BilibiliApi.Login;
namespace Test;

public class QrCodeLoginTest {
    [Fact]
    public async void IsWorkWell() {
        Console.WriteLine("Test Start");
        var res = await QrCodeLogin.ApplyForQRCode();
        Console.WriteLine(res.Item1 + Environment.NewLine + res.Item2);
    }
}