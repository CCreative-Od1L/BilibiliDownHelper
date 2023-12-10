using Core.BilibiliApi.Login.Model;
using Core.Utils;

namespace Core.BilibiliApi.User {
    static public class UserManager {
        static public bool IsLogin { get; private set; }
        static public string RefreshToken { get; private set; }
        static DateTime RefreshTokenUpdateTime;
        static UserManager() {
            RefreshToken = string.Empty;
        }
        static public void UpdateUserLoginInfo(QRCodeLoginResponse response, Action? callback = null) {
            IsLogin = true;
            RefreshToken = response.Data.RefreshToken;
            RefreshTokenUpdateTime = DateTimeUtils.TimestampToDateTime(Convert.ToString(response.Data.Timestamp));
            
            // * 建个线程去做网络请求，更新用户数据
            new Task(() => {
                AutoResetEvent webRequestLock = new(false);
                GetBilibiliUserInfoAsync(webRequestLock);
                webRequestLock.WaitOne();
                callback?.Invoke();
            }).Start();
        }
        static async void GetBilibiliUserInfoAsync(AutoResetEvent webRequestLock) {
            string url = "https://api.bilibili.com/x/web-interface/nav";
            string methodName = "get";

            var result = await Web.WebClient.Request(url, methodName);
            if (result.Item1 == false) {
                CoreManager.logger.Info(nameof(GetBilibiliUserInfoAsync), result.Item2);
            } else {
                // TODO 整理返回数据
                
                webRequestLock.Set();
            }
        }
    }
}