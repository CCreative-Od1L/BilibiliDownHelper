using Core.BilibiliApi.Login;
using Core.BilibiliApi.User;
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
        public UserService() {
            UserInfoAPI.INSTANCE.AddMyInfoUpdateListener(UserInfoUpateAction);
        }
        public static void OpenService() {
            Task.Run(async () => {
                await UserInfoAPI.INSTANCE.TryToUpdateMyInfoAsync();
            });
        }
        // TODO 注销操作忘做了
        public void UserInfoUpateAction(LoginUserInfoData data) {
            IsLoggedIn = true;
            CurrentUserInfo = data;
        }
        public static void LoginByQRCode(Action<byte[]> loadAction) {
            QrCodeLoginAPI.LoginByQrCode(loadAction);
        }
    }
}
