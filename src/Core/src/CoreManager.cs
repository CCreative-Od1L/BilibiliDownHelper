using Core.Cookie;
using Core.Directory;
using Core.Logger;

namespace Core {
    static class CoreManager {
        static public readonly CookieManager cookieMgr;
        static public readonly LogManager logger;
        static public readonly DirectoryManager directoryMgr;
        static CoreManager(){
            directoryMgr = new();
            logger = new();
            
            cookieMgr = new();
        }
    }
}