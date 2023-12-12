using System.ComponentModel;
using Core.BilibiliApi.Login.Model;
using Core.Utils;

namespace Core.BilibiliApi.User {
    static public class UserManager {
        static public bool IsLogin { get; private set; }
        static public string RefreshToken { get; private set; }
        static DateTime RefreshTokenUpdateTime { get; set; }
        static public UserInfo? NowUserInfo { get; private set; }
        static event Action? OnUserInfoUpdate;
        static UserManager() {
            IsLogin = false;
            OnUserInfoUpdate = null;
            RefreshToken = string.Empty;
            NowUserInfo = null;
        }
        static public bool AddUserInfoUpdateListener(Action action) {
            try {
                OnUserInfoUpdate += action;
                CoreManager.logger.Info(string.Format("增加用户信息更新订阅，函数：{0}", nameof(action)));
                return true;
            } catch (Exception e) {
                CoreManager.logger.Error(nameof(AddUserInfoUpdateListener), e);
                return false;
            }
        }
        static public bool RemoveUserInfoUpdateListener(Action action) {
            try {
                OnUserInfoUpdate -= action;
                CoreManager.logger.Info(string.Format("取消用户信息更新订阅，函数{0}", nameof(action)));
                return true;
            } catch (Exception e) {
                CoreManager.logger.Error(nameof(RemoveUserInfoUpdateListener), e);
                return false;
            }
        }
        static public void UpdateUserLoginInfo(QRCodeLoginResponse response) {
            IsLogin = true;
            RefreshToken = response.Data.RefreshToken;
            RefreshTokenUpdateTime = DateTimeUtils.TimestampToDateTime(Convert.ToString(response.Data.Timestamp));
            
            // * 建个线程去做网络请求，更新用户数据
            new Task(async () => {
                var webResponse = await GetBilibiliUserInfoAsync();
                if (LoadUserInfo(webResponse)) {
                    OnUserInfoUpdate?.Invoke();
                }
            }).Start();
        }
        /// <summary>
        /// 由外部调用，用于在尝试使用已有的Cookie文件进行用户信息的获取。
        /// </summary>
        static public void TryToUpdateUserLoginInfo(Action? onSuccessCallback, Action? onFailureCallback) {
            new Task (async () => {
                var webResponse = await GetBilibiliUserInfoAsync();
                if (!string.IsNullOrEmpty(webResponse)) {
                    if (LoadUserInfo(webResponse)) {
                        onSuccessCallback?.Invoke();
                        OnUserInfoUpdate?.Invoke();
                        return;
                    }
                }
                onFailureCallback?.Invoke();
            }).Start();
        }
        static async Task<string> GetBilibiliUserInfoAsync() {
            string url = @"https://api.bilibili.com/x/web-interface/nav";
            string methodName = "get";

            var result = await Web.WebClient.Request(url, methodName);
            if (result.Item1 == false) {
                CoreManager.logger.Info(nameof(GetBilibiliUserInfoAsync), result.Item2);
            } else {
                return result.Item2;
            }
            return string.Empty;
        }
        static bool LoadUserInfo(string? response) {
            if (string.IsNullOrEmpty(response)) { return false; }
            var userInfoResponse = JsonUtils.ParseJsonString<UserInfoResponse>(response);
            if(userInfoResponse != null) {
                if (userInfoResponse.CheckIsValid()) {
                    NowUserInfo = userInfoResponse.userInfo;
                    CoreManager.logger.Info("用户信息装填成功。");
                    return true;
                } else {
                    CoreManager.logger.Info(userInfoResponse.Message ?? "获取用户数据失败。");
                }
            }
            return false;
        }
    }
}