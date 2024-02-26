using BvDownkr.src.Implement;
using BvDownkr.src.Models;
using BvDownkr.src.Services;
using BvDownkr.src.Utils;
using BvDownkr.src.Views;
using Core;
using Core.BilibiliApi.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BvDownkr.src.ViewModels {
    public class MainWindowVM : NotificationObject {
        private readonly MainWindowModel _model;
        private readonly MainWindow _window;
        public string currentOpenFrameName = string.Empty;
        public MainWindowVM() {
            _model = new();
            _window = (MainWindow)Application.Current.MainWindow;
            UserService.INSTANCE.AddUpdateUserInfoUIAction(
                action: UpdateUserAvatar,
                nameof(MainWindowVM));
        }
        public Visibility MaskVisible {
            get => _model.IsMaskVisible;
            set {
                _model.IsMaskVisible = value;
                RaisePropertyChanged(nameof(MaskVisible));
            }
        }
        public Visibility UserInfoPanelVisible {
            get => _model.IsUserInfoPanelVisible;
            set {
                _model.IsUserInfoPanelVisible = value;
                RaisePropertyChanged(nameof(UserInfoPanelVisible));
            }
        }
        public Visibility DownloadTaskPanelVisible {
            get => _model.IsDownloadTaskPanelVisible;
            set {
                _model.IsDownloadTaskPanelVisible = value;
                RaisePropertyChanged(nameof(DownloadTaskPanelVisible));
            }
        }
        public Visibility Area1Visible {
            get => _model.IsArea1Visible;
            set {
                _model.IsArea1Visible = value;
                RaisePropertyChanged(nameof(Area1Visible));
            }
        }
        public Brush? TopBarButtonBackground {
            get => _model.TopBarButtonBackground;
            set {
                _model.TopBarButtonBackground = value;
                RaisePropertyChanged(nameof(TopBarButtonBackground));
            }
        }
        public ImageSource? UserAvatar {
            get => _model.UserAvatar;
            set {
                _model.UserAvatar = value;
                RaisePropertyChanged(nameof(UserAvatar));
            }
        }
        public ICommand OnMouseEnterTopBarButton => new ReplyCommand<object>(
            (_) => {
                TopBarButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e3e3e3"));
            }, true);
        public ICommand OnMouseLeaveTopBarButton => new ReplyCommand<object>(
            (_) => {
                TopBarButtonBackground = null;
            }, true);
        public static ICommand CloseProgram => new ReplyCommand<object>(
            async (_) => {
                // TODO 打开Loading UI，禁止用户操作
                await DownloadService.INSTANCE.CloseServer();
                Application.Current.Shutdown();
            }, true);
        public ICommand CloseFrame => new ReplyCommand<Rectangle>(
            (Rectangle? mask) => {
                if (!string.IsNullOrEmpty(currentOpenFrameName) && mask != null) {
                    switch(currentOpenFrameName) {
                        case nameof(DownloadTaskPanelVisible):
                            DownloadTaskPanelVisible = Visibility.Hidden;
                            break;
                        case nameof(UserInfoPanelVisible):
                            UserInfoPanelVisible = Visibility.Hidden;
                            break;
                    }
                    
                    MaskVisible = Visibility.Hidden;
                    currentOpenFrameName = string.Empty;
                }
            }, true);
        public ICommand OpenUserInfoFrame => new ReplyCommand<object>(
            (_) => {
                currentOpenFrameName = nameof(UserInfoPanelVisible);

                UserInfoPanelVisible = Visibility.Visible;
                MaskVisible = Visibility.Visible;
            }, true);
        public ICommand OpenDownloadTaskFrame => new ReplyCommand<object>(
            (_) => {
                currentOpenFrameName = nameof(DownloadTaskPanelVisible);

                DownloadTaskPanelVisible = Visibility.Visible;
                MaskVisible = Visibility.Visible;
            }, true);
        private void UpdateUserAvatar(LoginUserInfoData data, byte[] rawAvatarData) {
            _window.Dispatcher.Invoke(() => {
                UserAvatar = UIMethod.GetBitmapSource(rawAvatarData);
            });
        }
    }
}
