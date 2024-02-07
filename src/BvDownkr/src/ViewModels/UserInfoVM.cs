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

namespace BvDownkr.src.ViewModels {
    public class UserInfoVM : NotificationObject {
        private readonly UserInfoModel _model;
        public UserInfoVM() {
            _model = new();
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

        public ICommand OnPageLoaded => new ReplyCommand<object>(
            (_) => {
                if (!UserService.INSTANCE.IsLoggedIn) {
                    QRcodeLoginPage = new QRCodeLoginPage();
                    QRcodeLoginPageVisiable = Visibility.Visible;
                }
            }, true);
    }
}
