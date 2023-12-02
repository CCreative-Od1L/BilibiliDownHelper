using System.Globalization;
using Core;
using Xunit.Abstractions;

namespace Core.Test {
    public class CookieManagerTest {
        readonly ITestOutputHelper output;
        public CookieManagerTest(ITestOutputHelper testOutputHelper) {
            output = testOutputHelper;
        }
        [Fact]
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
                    "Fri, 24 May 2024 11:00:42 GMT".Replace("GMT", "+0"),
                    "ddd, dd MMM yyyy hh:mm:ss z", 
                    CultureInfo.GetCultureInfo("en-us")).ToString());
        }
    }
}