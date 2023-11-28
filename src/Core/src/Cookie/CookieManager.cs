using Core.Directory;
using Core.Logger;

namespace Core.Cookie {
    static public class CookieManager {
        static readonly string CookieDirectory;
        static string _cookieFilePath;
        static public string CookieFilePath {
            get => _cookieFilePath; 
            private set {
                _cookieFilePath = value;
            }
        }
        static public Dictionary<string, CookieData>? Cookies {get; private set;}
        static CookieManager() {
            Cookies = [];
            CookieDirectory = DirectoryManager.GetCookieDirectory();
            if (!System.IO.Directory.Exists(CookieDirectory)) {
                System.IO.Directory.CreateDirectory(CookieDirectory);
            } 
            _cookieFilePath = Path.Combine(DirectoryManager.GetCookieDirectory(), "cookies");
        }

        static public bool CheckCookieFileExist() {
            return File.Exists(_cookieFilePath);
        }

        static public void UpdateCookiesData() {
            {
                using var sr = new StreamReader(_cookieFilePath);
                string? line = string.Empty;
                while((line = sr.ReadLine()) != null) {
                    CookieData cookieData = new(line);
                    if (string.IsNullOrEmpty(cookieData.CookieName)) {
                        LogManager.Info(nameof(CookieData), "初始化cookie失败, cookieData.CookieName为空。");
                        return;
                    }
                    if (!Cookies!.TryAdd(cookieData.CookieName, cookieData)) {
                        Cookies[cookieData.CookieName] = cookieData;
                    }
                }
            }
        }
    }
}