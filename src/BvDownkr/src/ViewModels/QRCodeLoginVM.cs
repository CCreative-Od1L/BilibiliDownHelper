using BvDownkr.src.Implement;
using BvDownkr.src.Models;
using BvDownkr.src.Utils;
using Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using BvDownkr.src.Services;

namespace BvDownkr.src.ViewModels {
    internal class QRCodeLoginVM : NotificationObject {
        private readonly QRCodeLoginModel _model;
        private KrTimer? _qrcodeRefreshReminder = null;
        public QRCodeLoginVM() {
            _model = new();
        }
        private void OpenKrTimer() {
            _qrcodeRefreshReminder ??= new(RemindQRcodeRefresh, 60, false);
            _qrcodeRefreshReminder.Reset();
        }
        private void RemindQRcodeRefresh() {
            CoreManager.logger.Debug("登录二维码超时");
            QRcodeBlurEffectRadius = 15;
            ShowRefreshUI();
        }
        /// <summary>
        /// * 已扫描UI展示
        /// </summary>
        private void ShowScanedUI() {
            QRcodeScanedUIVisiable = Visibility.Visible;
        }
        /// <summary>
        /// * 已扫描UI关闭
        /// </summary>
        private void CloseScanedUI() {
            QRcodeScanedUIVisiable = Visibility.Hidden;
        }
        /// <summary>
        /// * 二维码超时UI展示
        /// </summary>
        private void ShowRefreshUI() {
            QRcodeRefreshUIVisiable = Visibility.Visible;
        }
        /// <summary>
        /// * 二维码超时UI关闭
        /// </summary>
        private void CloseRefreshUI() {
            QRcodeRefreshUIVisiable = Visibility.Hidden;
        }
        /// <summary>
        /// * 二维码图像装载
        /// </summary>
        /// <param name="rawData"></param>
        private void LoadQRcodeAction(byte[] rawData) {
            QRCodeImageSource = UIMethod.GetBitmapSource(rawData);
        }
        public ImageSource? QRCodeImageSource {
            get => _model.QRcodeImageSource;
            set {
                _model.QRcodeImageSource = value;
                RaisePropertyChanged(nameof(QRCodeImageSource));
            }
        }
        public Visibility QRcodeScanedUIVisiable {
            get => _model.IsQRcodeScanedUIVisiable;
            set {
                _model.IsQRcodeScanedUIVisiable = value;
                RaisePropertyChanged(nameof(QRcodeScanedUIVisiable));
            }
        }
        public Visibility QRcodeRefreshUIVisiable {
            get => _model.IsQRcodeRefreshUIVisiable;
            set {
                _model.IsQRcodeRefreshUIVisiable = value;
                RaisePropertyChanged(nameof(QRcodeRefreshUIVisiable));
            }
        }
        public int QRcodeBlurEffectRadius {
            get => _model.QRcodeBlurEffRadius;
            set {
                _model.QRcodeBlurEffRadius = value;
                RaisePropertyChanged(nameof(QRcodeBlurEffectRadius));
            }
        }
        public string QRcodeRemindText {
            get => _model.QRcodeRemindText;
            set {
                _model.QRcodeRemindText = value;
                RaisePropertyChanged(nameof(QRcodeRemindText));
            }
        }
        public void RefreshQRcodeAction() {
            // * 打开时间记录
            OpenKrTimer();
            // * UI更新
            QRcodeBlurEffectRadius = 0;
            CloseScanedUI();
            CloseRefreshUI();
            // * 服务请求
            UserService.LoginByQRCode(
                loadAction: LoadQRcodeAction,
                qrcodeScanCallback: ShowScanedUI);
        }
        public ICommand RefreshQRcode => new ReplyCommand<object>(
            (_) => {
                RefreshQRcodeAction();
            }, true);
        public ICommand OnPageLoaded => new ReplyCommand<object>(
            (_) => {
                // * Text Load
                QRcodeRemindText = "打开bilibili客户端扫一扫";
                RefreshQRcodeAction();
            }, true);
    }
}
