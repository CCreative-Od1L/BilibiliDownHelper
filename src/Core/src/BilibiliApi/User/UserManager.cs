using System.ComponentModel;
using Core.BilibiliApi.Login.Model;
using Core.Utils;

namespace Core.BilibiliApi.User {
    /// <summary>
    /// * 用户信息管理-单例模式
    /// </summary>
    public class UserManager {
        public static UserManager Instance { get; } = new();
        public bool IsLogin { get; private set; }
        public UserInfo? NowUserInfo { get; private set; }
        private event Action? OnUserInfoUpdate;
        private UserManager() {
            IsLogin = false;
            OnUserInfoUpdate = null;
            NowUserInfo = null;

            // TODO 支持登录记忆
            // * TryToUpdateUserLoginInfo();
        }
        public bool AddUserInfoUpdateListener(Action action) {
            try {
                OnUserInfoUpdate += action;
                CoreManager.logger.Info(string.Format("增加用户信息更新订阅，函数：{0}", nameof(action)));
                return true;
            } catch (Exception e) {
                CoreManager.logger.Error(nameof(AddUserInfoUpdateListener), e);
                return false;
            }
        }
        public bool RemoveUserInfoUpdateListener(Action action) {
            try {
                OnUserInfoUpdate -= action;
                CoreManager.logger.Info(string.Format("取消用户信息更新订阅，函数{0}", nameof(action)));
                return true;
            } catch (Exception e) {
                CoreManager.logger.Error(nameof(RemoveUserInfoUpdateListener), e);
                return false;
            }
        }
        public void UpdateUserLoginInfo(QRCodeLoginResponse response) {
            IsLogin = true;
            CoreManager.cookieMgr.UpdateRefreshToken(response.Data.RefreshToken);
            CoreManager.cookieMgr.UpdateRefreshTokenTime(DateTimeUtils.TimestampToDateTime(Convert.ToString(response.Data.Timestamp)));
            
            // * 建个线程去做网络请求，更新用户数据
            new Task(async () => {
                var webResponse = await GetBilibiliUserInfoAsync();
                if (LoadUserInfo(webResponse)) {
                    OnUserInfoUpdate?.Invoke();
                }
            }).Start();
        }
        /// <summary>
        /// * 由外部调用，用于在尝试使用已有的Cookie文件进行用户信息的获取。
        /// </summary>
        public void TryToUpdateUserLoginInfo(Action? onSuccessCallback = null, Action? onFailureCallback = null) {
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
        /// <summary>
        /// * 异步，获取用户信息
        /// </summary>
        /// <returns>返回请求json文本结果</returns>
        private async Task<string> GetBilibiliUserInfoAsync() {
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
        /// <summary>
        /// * 用户信息装填
        /// </summary>
        /// <param name="response"></param>
        /// <returns>是否成功装填</returns>
        private bool LoadUserInfo(string? response) {
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