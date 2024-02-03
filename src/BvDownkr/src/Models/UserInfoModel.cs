using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BvDownkr.src.Models {
    public class UserInfoModel {
        public Page? QRcodeLoginPage { get; set; } = null;
        public Visibility IsQRcodeLoginPageVisiable { get; set; } = Visibility.Hidden;
    }
}
