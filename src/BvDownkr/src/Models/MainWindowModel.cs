﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BvDownkr.src.Models {
    public class MainWindowModel {
        public Button? UserButton { get; set; }
        public Rectangle? Mask { get; set; }
        public Frame? UserInfoPanel { get; set; }
        public Frame? DownloadTaskPanel { get; set; }
    }
}
