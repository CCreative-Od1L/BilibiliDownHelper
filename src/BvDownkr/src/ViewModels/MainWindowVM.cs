using BvDownkr.src.Models;
using BvDownkr.src.Services;
using BvDownkr.src.Utils;
using BvDownkr.src.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace BvDownkr.src.ViewModels
{
    public class MainWindowVM {
        public MainWindowModel _model;
        public Grid? currentOpenFrame = null;
        public MainWindowVM() {
            _model = new MainWindowModel();
        }
        public MainWindowVM(MainWindowModel model) {
            _model = model;
        }

        public static ICommand CloseProgram => new ReplyCommand<object>(
            (_) => {
                Application.Current.Shutdown();
            }, true);
        public ICommand CloseFrame => new ReplyCommand<Rectangle>(
            (Rectangle? mask) => {
                if (currentOpenFrame != null && mask != null) {
                    currentOpenFrame.Visibility = Visibility.Hidden;
                    mask.Visibility = Visibility.Hidden;

                    currentOpenFrame = null;
                }
            }, true);
        public ICommand OpenUserInfoFrame => new ReplyCommand<object>(
            (_) => {
                currentOpenFrame = _model.UserInfoPanel;

                if(UserService.Instance.IsLoggedIn) {
                    _model.UserInfoContent!.Content = PageManager.UserInfoPage;
                } else {
                    _model.UserInfoContent!.Content = PageManager.QRCodeLoginPage;
                }

                _model.UserInfoPanel!.Visibility = Visibility.Visible;
                _model.Mask!.Visibility = Visibility.Visible;
            }, true);
        public ICommand OpenDownloadTaskFrame => new ReplyCommand<object>(
            (_) => {
                currentOpenFrame = _model.DownloadTaskPanel;
                
                _model.DownloadTaskContent!.Content = PageManager.DownloadTaskPage;
                _model.DownloadTaskPanel!.Visibility = Visibility.Visible;
                _model.Mask!.Visibility = Visibility.Visible;
            }, true);
    }
}
