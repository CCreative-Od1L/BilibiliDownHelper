using BvDownkr.src.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BvDownkr.src.Models {
    public class UserInfoModel {
        public Page? QRcodeLoginPage { get; set; } = null;
        public Visibility IsQRcodeLoginPageVisiable { get; set; } = Visibility.Hidden;
        public UserInfoEntry UserInfo { get; set; } = new();
        public ImageSource? UserAvatar { get; set; }
        public ImageSource? LevelIcon { get; set; }
        public Visibility SeniorIconEnable { get; set; } = Visibility.Hidden;
        public Visibility VipIconEnable { get; set; } = Visibility.Hidden;
        public string MaxVideoQn { get; set; } = string.Empty;
        public string MaxAudioQn { get; set; } = string.Empty;
    }
}
