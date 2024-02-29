using BvDownkr.src.Implement;
using BvDownkr.src.Services;
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
        public string VideoTitle { get; set; } = string.Empty;
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
        private string BuildFileName() {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(VideoTitle);
            stringBuilder.Append('_');
            stringBuilder.Append(PageName);
            stringBuilder.Append(".mp4");

            return stringBuilder.ToString();
        }
        public ICommand StartDownload => new ReplyCommand<object>(
            (_) => {
                CoreManager.logger.Debug(string.Format("VideoSelect: {0}, AudioSelect: {1}", VideoQnSelected, AudioQnSelected));
                
                var videoSLinks = VideoDownLinks[VideoQnSelected];
                var audioSLinks = AudioDownLinks[AudioQnSelected];
                var finalFileName = BuildFileName();
                // Configure open folder dialog box
                Microsoft.Win32.OpenFolderDialog dialog = new() {
                    Multiselect = false,
                    Title = "选择保存的文件夹"
                };

                // Show open folder dialog box
                bool? result = dialog.ShowDialog();

                // Process open folder dialog box results
                if (result == true) {
                    // Get the selected folder
                    string fullPathToFolder = dialog.FolderName;
                    DownloadService.INSTANCE.DownloadVideo(
                        videoLinks: videoSLinks,
                        audioLinks: audioSLinks,
                        finalFileName: finalFileName,
                        fileDir: fullPathToFolder);
                }

            }, true);
    }
}
