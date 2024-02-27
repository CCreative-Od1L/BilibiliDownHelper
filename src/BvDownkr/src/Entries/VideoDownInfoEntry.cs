using BvDownkr.src.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BvDownkr.src.Entries
{
    public class VideoDownInfoEntry : NotificationObject {
        public string No { get; set; } = string.Empty;
        public string PageName { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public Dictionary<int, string> VideoQnDic { get; set; } = [];
        public Dictionary<int, string> AudioQnDic { get; set; } = [];

        private string _videoQnSelected = string.Empty;
        public string VideoQnSelected {
            get => _videoQnSelected;
            set {
                _videoQnSelected = value;
                RaisePropertyChanged(nameof(VideoQnSelected));
            }
        }
        private string _audioQnSelected = string.Empty;
        public string AudioQnSelected {
            get => _audioQnSelected;
            set {
                _audioQnSelected = value;
                RaisePropertyChanged(nameof(AudioQnSelected));
            }
        }
        public ICommand StartDownload => new ReplyCommand<object>(
            (_) => {

            }, true);
    }
}
