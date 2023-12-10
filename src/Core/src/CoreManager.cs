using Core.CookieFunc;
using Core.DirectoryFunc;
using Core.Logger;

namespace Core {
    public static class CoreManager {
        static public readonly CookieManager cookieMgr;
        static public readonly LogManager? logger;
        static public readonly DirectoryManager directoryMgr;
        static CoreManager(){
            // !! 注意初始化的顺序
            directoryMgr = new();
            logger = new();
            
            cookieMgr = new();
        }
    }
}