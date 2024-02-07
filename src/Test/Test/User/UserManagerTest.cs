using System.Globalization;
using Core;
using Xunit.Abstractions;

namespace Core.Test {
    public class UserManagerTest(ITestOutputHelper testOutputHelper)
    {
        readonly ITestOutputHelper output = testOutputHelper;

        // [Fact]
        public async void IsUpdateCookiesDataWorkWell() {
            CoreManager.logger.Info("Main", "Logger start");
            output.WriteLine("System start");

            string url = "https://api.bilibili.com/x/web-interface/nav";
            string methodName = "get";

            output.WriteLine("Web Request Start");
            var result = await Web.WebClient.RequestJson(url: url, methodName: methodName);
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
                TimeStringToDateTime(
                    "Thu, 30 May 2024 13:33:42 GMT".Replace("GMT", "+0"),
                    //"Thu, 30 May 2024 17:33:40 +0",
                    "ddd, dd MMM yyyy HH:mm:ss z").ToString()
            );
            
            var timestampStr = "1702214445183";
            output.WriteLine(TimestampToDateTime(timestampStr).ToLocalTime().ToString());
        }

        // * TimestampUtils.cs GetCurrentTimestamp
        string GetCurrentTimestampSecond() {
            return Convert.ToString(
                Convert.ToInt64(
                    (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds
                )
            );
        }

        // * TimestampUtils.cs TimestampToDateTime
        DateTime TimestampToDateTime(string timestampStr) {
            var timestamp = long.Parse(timestampStr);
            string currentTimestamp = GetCurrentTimestampSecond();
            if (timestampStr.Length > currentTimestamp.Length) {
                return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime.ToLocalTime();
            } else {
                return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToLocalTime();
            }
        }

        // * TimestampUtils.cs TimeStringToDateTime
        DateTime TimeStringToDateTime(string timeString, string format) {
            return DateTime.ParseExact(
                        timeString,
                        format,
                        CultureInfo.GetCultureInfo("en-us"));
        }
    }
}