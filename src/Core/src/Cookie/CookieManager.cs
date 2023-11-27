using Core.Directory;

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
        static public List<CookieData>? Cookies {get; private set;}
        static CookieManager() {
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
            
        }
    }
}