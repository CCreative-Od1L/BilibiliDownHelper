using Core.Cookie;
using Xunit.Abstractions;

namespace Core.Test {
    public class CookieManagerTest {
        readonly ITestOutputHelper output;
        public CookieManagerTest(ITestOutputHelper testOutputHelper) {
            output = testOutputHelper;
        }
        [Fact]
        public void IsUpdateCookiesDataWorkWell() {
            string filePath = CookieManager.CookieFilePath;
            {
                using var sr = new StreamReader(filePath);
                string? line = string.Empty;
                while((line = sr.ReadLine()) != null) {
                    CookieData cookieData = new(line); 
                    output.WriteLine(cookieData.ToString());
                }
            }
        }
    }
}