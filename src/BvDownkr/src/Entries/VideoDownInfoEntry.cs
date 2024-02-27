using BvDownkr.src.Implement;
using Core;
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
        public List<string> VideoQnList { get; set; } = [];
        public List<string> AudioQnList { get; set; } = [];
        public List<List<string>> VideoDownLinks { get; set; } = [];
        public List<List<string>> AudioDownLinks { get; set; } = [];
        private int _videoQnSelected;
        public int VideoQnSelected {
            get => _videoQnSelected;
            set {
                _videoQnSelected = value;
                RaisePropertyChanged(nameof(VideoQnSelected));
            }
        }
        private int _audioQnSelected;
        public int AudioQnSelected {
            get => _audioQnSelected;
            set {
                _audioQnSelected = value;
                RaisePropertyChanged(nameof(AudioQnSelected));
            }
        }
        public ICommand StartDownload => new ReplyCommand<object>(
            (_) => {
                CoreManager.logger.Debug(string.Format("VideoSelect: {0}, AudioSelect: {1}", VideoQnSelected, AudioQnSelected));
            }, true);
    }
}
