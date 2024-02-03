using BvDownkr.src.Models;
using BvDownkr.src.Services;
using BvDownkr.src.Utils;
using BvDownkr.src.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace BvDownkr.src.ViewModels
{
    public class MainWindowVM : NotificationObject {
        private readonly MainWindowModel _model;
        public string currentOpenFrameName = string.Empty;
        public MainWindowVM() {
            _model = new();
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


        public static ICommand CloseProgram => new ReplyCommand<object>(
            (_) => {
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
    }
}
