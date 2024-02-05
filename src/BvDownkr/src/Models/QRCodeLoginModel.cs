using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BvDownkr.src.Models {
    public class QRCodeLoginModel {
        public ImageSource? QRcodeImageSource { get; set; }
        public int QRcodeBlurEffRadius { get; set; } = 0;
        public Visibility IsQRcodeScanedUIVisiable { get; set; } = Visibility.Hidden;
        public Visibility IsQRcodeRefreshUIVisiable {  get; set; } = Visibility.Hidden;

        public string QRcodeRemindText { get; set; } = string.Empty;
    }
}
