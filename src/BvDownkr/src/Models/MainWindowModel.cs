using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BvDownkr.src.Models {
    public class MainWindowModel {
        public Visibility IsMaskVisible { get; set; } = Visibility.Hidden;
        public Visibility IsUserInfoPanelVisible { get; set; } = Visibility.Hidden;
        public Visibility IsDownloadTaskPanelVisible { get; set; } = Visibility.Hidden;
        public Visibility IsArea1Visible { get; set; } = Visibility.Hidden;
        public Brush? TopBarButtonBackground { get; set; } = null;
        public ImageSource? UserAvatar { get; set; }
        public Page? Area2Page { get; set; }
    }
}
