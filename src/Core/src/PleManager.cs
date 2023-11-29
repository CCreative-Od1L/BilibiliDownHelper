using Core.Cookie;

namespace Core {
    static class PleManager {
        static public CookieManager cookieManager;
        static PleManager(){
            cookieManager = new();
        }
    }
}