using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BvDownkr.src.Models {
    public class VideoDescModel {
        public ImageSource? VideoCover { get; set; }
        public ImageSource? OwnerAvatar { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
    }
}
