using BvDownkr.src.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using BvDownkr.src.Models;
using BvDownkr.src.Views;
using System.Windows.Input;
using BvDownkr.src.Services;
using BvDownkr.src.Implement;
using Core.BilibiliApi.User;
using BvDownkr.src.Entries;
using System.Windows.Media;
using Org.BouncyCastle.Asn1.Pkcs;
using Core.Web;
using Newtonsoft.Json.Linq;

namespace BvDownkr.src.ViewModels {
    public class UserInfoVM : NotificationObject {
        private readonly UserInfoModel _model;
        public UserInfoVM() {
            _model = new();
            UserService.INSTANCE.AddUpdateUserInfoUIAction(
                action: UpdateUserInfoUI,
                className: nameof(UserInfoVM));
        }
        public string UId {
            get => _model.UserInfo.UID;
            set {
                _model.UserInfo.UID = value;
                RaisePropertyChanged(nameof(UId));
            }
        }
        public ImageSource? UserAvatar {
            get => _model.UserAvatar;
            set {
                _model.UserAvatar = value;
                RaisePropertyChanged(nameof(UserAvatar));
            }
        }
        public ImageSource? LevelIcon {
            get => _model.LevelIcon;
            set {
                _model.LevelIcon = value;
                RaisePropertyChanged(nameof(LevelIcon));
            }
        }
        public string UserName {
            get => _model.UserInfo.Uname;
            set {
                _model.UserInfo.Uname = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }
        public Page? QRcodeLoginPage {
            get => _model.QRcodeLoginPage;
            set { 
                _model.QRcodeLoginPage = value;
                RaisePropertyChanged(nameof(QRcodeLoginPage));
            }
        }
        public Visibility QRcodeLoginPageVisiable {
            get => _model.IsQRcodeLoginPageVisiable;
            set { 
                _model.IsQRcodeLoginPageVisiable = value;
                RaisePropertyChanged(nameof(QRcodeLoginPageVisiable));
            }
        }
        public Visibility SeniorIconEnable {
            get => _model.SeniorIconEnable;
            set {
                _model.SeniorIconEnable = value;
                RaisePropertyChanged(nameof(SeniorIconEnable));
            }
        }
        public Visibility VipIconEnable {
            get => _model.VipIconEnable;
            set {
                _model.VipIconEnable = value;
                RaisePropertyChanged(nameof(VipIconEnable));
            }
        }
        public ICommand OnPageLoaded => new ReplyCommand<object>(
            (_) => {
                if (!UserService.INSTANCE.IsLoggedIn) {
                    QRcodeLoginPage = new QRCodeLoginPage();
                    QRcodeLoginPageVisiable = Visibility.Visible;
                }
            }, true);
        private void UpdateUserInfoUI(LoginUserInfoData data, byte[] rawAvatarData) {
            if (data.IsLogin) {
                UId = data.Mid.ToString();
                UserName = data.Uname;
                if (data.LevelInfo != null) {
                    UpdateLevelIcon(
                        data.LevelInfo.CurrentLevel,
                        data.LevelInfo.CurrentMin,
                        data.LevelInfo.CurrentExp);
                }
                UpdateSeniorIcon(data.IsSenior());
                UpdateVipIcon(data.IsVIP());

                UserService.INSTANCE.CashUserAvatar(rawAvatarData);
                PageManager.UserInfoPage.Dispatcher.Invoke(() => {
                    UserAvatar = UIMethod.GetBitmapSource(rawAvatarData);
                });
                // * 关闭登录界面
                QRcodeLoginPageVisiable = Visibility.Hidden;
            }
        }
        public void UpdateLevelIcon(int currentLevel, int currentMin, int CurrentExp) {

        }
        public void UpdateSeniorIcon(bool isSenior) {
            SeniorIconEnable = isSenior ? Visibility.Visible : Visibility.Hidden;
        }
        public void UpdateVipIcon(bool isVip) {
            VipIconEnable = isVip ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
