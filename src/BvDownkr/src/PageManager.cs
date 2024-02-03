using BvDownkr.src.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src {
    public class PageManager {
        public static UserInfoPage UserInfoPage { get; private set; } = new();
        public static SearchPage SearchPage { get; private set; } = new();
        public static DownloadTaskPage DownloadTaskPage { get; private set; } = new();
    }
}
