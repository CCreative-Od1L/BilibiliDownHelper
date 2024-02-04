using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BvDownkr.src.Models {
    public class QRCodeLoginModel {
        public ImageSource? QRcodeImageSource { get; set; }
        public Visibility IsQRcodeMaskVisiable { get; set; } = Visibility.Hidden;
    }
}
