using System.Globalization;
using Core;
using Core.Utils;
using Xunit.Abstractions;

namespace Core.Test {
    public class CookieManagerTest(ITestOutputHelper testOutputHelper)
    {
        readonly ITestOutputHelper output = testOutputHelper;

        // [Fact]
        public async void IsUpdateCookiesDataWorkWell() {
            CoreManager.logger.Info("Main", "Logger start");
            output.WriteLine("System start");

            string url = "https://api.bilibili.com/x/web-interface/nav";
            string methodName = "get";

            output.WriteLine("Web Request Start");
            var result = await Web.WebClient.Request(url, methodName);
            if (result.Item1 == false) {
                output.WriteLine(result.Item2);
            } else {
                FileUtils.WriteText(
                    Path.Combine(CoreManager.directoryMgr.fileDirectory.UserInfo!, "userinfo.json"),
                    result.Item2);
                output.WriteLine(result.Item2);
            }

            Thread.Sleep(5000);
            output.WriteLine("System End");
        }
        // [Fact]
        public void DateTimeParse() {
            // GMT
            output.WriteLine(
                DateTime.ParseExact(
                    "Thu, 30 May 2024 13:33:42 GMT".Replace("GMT", "+0"),
                    //"Thu, 30 May 2024 17:33:40 +0",
                    "ddd, dd MMM yyyy HH:mm:ss z", 
                    CultureInfo.GetCultureInfo("en-us")).ToString());
        }
    }
}