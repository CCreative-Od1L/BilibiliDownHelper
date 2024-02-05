using BvDownkr.src.Views;
using Core.BilibiliApi.Login;
using Core.BilibiliApi.User;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src.Services
{
    class UserService {
        public static UserService Instance { get; private set; } = new();
        public bool IsLoggedIn { get; private set; } = false;
        public LoginUserInfoData? CurrentUserInfo { get; private set; } = null;
        private event Action? OnUpdateUI;

        public UserService() {
            UserInfoAPI.INSTANCE.AddMyInfoUpdateListener(UserInfoUpateAction);
        }
        public static void OpenService() {
            Task.Run(async () => {
                await UserInfoAPI.INSTANCE.TryToUpdateMyInfoAsync();
            });
        }
        public void AddUpdateUIAction(Action action) { OnUpdateUI += action; }
        public void RemoveUpdateUIAction(Action action) { OnUpdateUI -= action; }
        // TODO 注销操作忘做了
        public void UserInfoUpateAction(LoginUserInfoData data) {
            IsLoggedIn = true;
            CurrentUserInfo = data;
            // * TODO 更新UI
            OnUpdateUI?.Invoke();
        }
        public static void LoginByQRCode(
            Action<byte[]> loadAction,
            Action? qrcodeScanCallback) {
            QrCodeLoginAPI.LoginByQrCode(loadAction, qrcodeScanCallback);
        }
    }
}
