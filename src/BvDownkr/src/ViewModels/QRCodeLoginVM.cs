using BvDownkr.src.Models;
using BvDownkr.src.Services;
using BvDownkr.src.Utils;
using BvDownkr.src.Implement;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.Windows.Interop;

namespace BvDownkr.src.ViewModels {
    public class QRCodeLoginVM : NotificationObject {
        private readonly QRCodeLoginModel _model;
        private KrTimer? _qrcodeRefreshReminder = null;
        public QRCodeLoginVM() {
            _model = new();
        }
        private void OpenKrTimer() {
            _qrcodeRefreshReminder ??= new(RemindQRcodeRefresh, 10, false);
            _qrcodeRefreshReminder.Reset();
        }
        private void RemindQRcodeRefresh() {
            CoreManager.logger.Debug("登录二维码超时");
            QRCodeMaskVisiable = Visibility.Visible;
        }
        public ImageSource? QRCodeImageSource {
            get => _model.QRcodeImageSource;
            set {
                _model.QRcodeImageSource = value;
                RaisePropertyChanged(nameof(QRCodeImageSource));
            }
        }
        public Visibility QRCodeMaskVisiable {
            get => _model.IsQRcodeMaskVisiable;
            set {
                _model.IsQRcodeMaskVisiable = value;
                RaisePropertyChanged(nameof(QRCodeMaskVisiable));
            }
        }
        public ICommand OnPageLoaded => new ReplyCommand<object>(
            (_) => {
                // * 打开时间记录
                OpenKrTimer();
                QRCodeMaskVisiable = Visibility.Hidden;
                UserService.LoginByQRCode(
                    (byte[] rawData) => {
                        Bitmap? bitmap = null;
                        using (MemoryStream ms = new(rawData)) {
                            bitmap = (Bitmap)Image.FromStream(ms);
                        }

                        IntPtr hBitmap = bitmap.GetHbitmap();
                        QRCodeImageSource = Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                    });
            }, true);
    }
}
