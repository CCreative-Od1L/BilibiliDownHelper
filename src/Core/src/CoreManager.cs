using Core.Aria2cNet;
using Core.BilibiliApi.User;
using Core.CookieFunc;
using Core.DirectoryFunc;
using Core.Logger;

namespace Core {
    public static class CoreManager {
        static public readonly CookieManager cookieMgr;
        static public readonly LogManager logger;
        static public readonly DirectoryManager directoryMgr;
        static public readonly AriaManager ariaMgr;
        static public readonly UserManager userMgr;
        static CoreManager(){
            // !! 注意初始化的顺序
            directoryMgr = DirectoryManager.INSTANCE;
            logger = LogManager.INSTANCE;
            
            cookieMgr = CookieManager.INSTANCE;
            ariaMgr = new();
            userMgr = UserManager.INSTANCE;
        }
    }
}