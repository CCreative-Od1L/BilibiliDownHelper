using BvDownkr.src.ViewModels;
using BvDownkr.src.Views;
using Core;
using Core.BilibiliApi.Login;
using Core.BilibiliApi.User;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src.Services
{
    class UserService {
        public static UserService INSTANCE { get; private set; } = new();
        public bool IsLoggedIn { get; private set; } = false;
        public event Action<LoginUserInfoData, byte[]>? OnUpdateUserInfoUI;
        private readonly string AvatarImageName = "avatar.jpg";
        private readonly SSwitchBase updateUserInfoSSwitch;
        
        public UserService() {
            string[] updateUserInfoConfigure = [
                nameof(UserInfoVM),
                nameof(MainWindowVM),
            ];

            UserInfoAPI.INSTANCE.AddMyInfoUpdateListener(UserInfoUpateAction);
            updateUserInfoSSwitch = new (updateUserInfoConfigure, () => {
                UserInfoAPI.INSTANCE.TryToUpdateMyInfoAsync();
            });
        }
        /// <summary>
        /// * 由VM调用
        /// </summary>
        /// <param name="action"></param>
        /// <param name="className"></param>
        public void AddUpdateUserInfoUIAction(
            Action<LoginUserInfoData, byte[]> action,
            string className) {
            OnUpdateUserInfoUI += action;
            updateUserInfoSSwitch.OpenSwitch(className);
        }
        // TODO 注销操作忘做了
        public void UserInfoUpateAction(LoginUserInfoData data, byte[] rawAvatarData) {
            IsLoggedIn = true;
            // * TODO 更新UI
            OnUpdateUserInfoUI?.Invoke(data, rawAvatarData);
        }
        public static void LoginByQRCode(
            Action<byte[]> loadAction,
            Action? qrcodeScanCallback) {
            QrCodeLoginAPI.LoginByQrCode(loadAction, qrcodeScanCallback);
        }
        public void CashUserAvatar(byte[] bAvatar) {
            FileUtils.WriteBytes(
                filePath: Path.Combine(
                    CoreManager.directoryMgr.fileDirectory.UserInfo!,
                    AvatarImageName),
                content: bAvatar);
        }
    }
}
