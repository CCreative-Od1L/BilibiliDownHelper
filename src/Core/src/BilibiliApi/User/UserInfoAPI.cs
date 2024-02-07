using System.ComponentModel;
using Core.BilibiliApi.Login.Model;
using Core.Utils;

namespace Core.BilibiliApi.User {
    /// <summary>
    /// * 用户信息管理-单例模式
    /// </summary>
    public class UserInfoAPI {
        public static UserInfoAPI INSTANCE { get; } = new();
        private event Action<LoginUserInfoData>? OnUserInfoUpdate = null;
        /// <summary>
        /// * 添加用户信息更新事件
        /// </summary>
        /// <param name="action"></param>
        /// <returns>布尔值，表示是否成功</returns>
        public bool AddMyInfoUpdateListener(Action<LoginUserInfoData> action) {
            try {
                OnUserInfoUpdate += action;
                CoreManager.logger.Info(string.Format("增加用户信息更新订阅，函数：{0}", nameof(action)));
                return true;
            } catch (Exception e) {
                CoreManager.logger.Error(nameof(AddMyInfoUpdateListener), e);
                return false;
            }
        }
        /// <summary>
        /// * 移除用户信息更新事件
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool RemoveMyInfoUpdateListener(Action<LoginUserInfoData> action) {
            try {
                OnUserInfoUpdate -= action;
                CoreManager.logger.Info(string.Format("取消用户信息更新订阅，函数{0}", nameof(action)));
                return true;
            } catch (Exception e) {
                CoreManager.logger.Error(nameof(RemoveMyInfoUpdateListener), e);
                return false;
            }
        }
        /// <summary>
        /// * 使用QRCode登录回应数据更新用户信息
        /// </summary>
        /// <param name="response"></param>
        public async Task UpdateMyInfoAsync(LoginResponseData data) {
            CoreManager.cookieMgr.UpdateRefreshTokenData(
                data.RefreshToken,
                Convert.ToString(data.Timestamp));
            CoreManager.cookieMgr.UpdateDataToFile();
            // * 做网络请求，更新用户数据
            var webResponse = await GetMyInfoAsync();
            var loadResult = LoadMyInfo(webResponse);
            if (loadResult != null) {
                OnUserInfoUpdate?.Invoke(loadResult);
            }
        }
        /// <summary>
        /// * 由外部调用，用于在尝试使用已有的Cookie文件进行用户信息的获取。
        /// </summary>
        public async Task TryToUpdateMyInfoAsync(Action? onSuccessCallback = null, Action? onFailureCallback = null) {
            var webResponse = await GetMyInfoAsync();
            if (!string.IsNullOrEmpty(webResponse)) {
                var loadResult = LoadMyInfo(webResponse);
                if (loadResult != null) {
                    onSuccessCallback?.Invoke();
                    OnUserInfoUpdate?.Invoke(loadResult);
                    return;
                }
            }
            onFailureCallback?.Invoke();
        }
        /// <summary>
        /// * 异步，获取用户信息
        /// </summary>
        /// <returns>返回请求json文本结果</returns>
        private async Task<string> GetMyInfoAsync() {
            string url = @"https://api.bilibili.com/x/web-interface/nav";
            string methodName = "get";

            var (isSuccess, content) = await Web.WebClient.RequestJson(url: url, methodName: methodName);
            if (!isSuccess) {
                CoreManager.logger.Info(nameof(GetMyInfoAsync), content);
            } else {
                return content;
            }
            return string.Empty;
        }
        /// <summary>
        /// * 用户信息装填
        /// </summary>
        /// <param name="response"></param>
        /// <returns>是否成功装填</returns>
        private static LoginUserInfoData? LoadMyInfo(string? response) {
            if (string.IsNullOrEmpty(response)) { return null; }
            var userInfoResponse = JsonUtils.ParseJsonString<LoginUserInfoResponse>(response);
            if(userInfoResponse != null) {
                if (userInfoResponse.IsValid()) {
                    CoreManager.logger.Info("用户信息装填成功。");
                    return userInfoResponse.Data;
                } else {
                    CoreManager.logger.Info(userInfoResponse.Message ?? "获取用户数据失败。");
                }
            }
            return null;
        }
    }
}