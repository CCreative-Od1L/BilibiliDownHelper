using BvDownkr.src.Implement;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BvDownkr.src.Entries
{
    public class BilibiliDownloadTaskEntry : NotificationObject {
        public string VGid { get; set; } = string.Empty;
        public string AGid { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string TaskRunMessage { get; set; } = "...";
        public string SaveFileDirPath { get; set; } = string.Empty;
        public string VideoTmpPath { get; set; } = string.Empty;
        public string AudioTmpPath { get; set; } = string.Empty;
        private double _videoDTaskValue = 0;
        public double VideoDTaskValue {
            get => _videoDTaskValue;
            set {
                _videoDTaskValue = value;
                RaisePropertyChanged(nameof(VideoDTaskValue));
            }
        }
        private double _audioDTaskValue = 0;
        public double AudioDTaskValue {
            get => _audioDTaskValue;
            set {
                _audioDTaskValue = value;
                RaisePropertyChanged(nameof(AudioDTaskValue));
            }
        }
        private double _taskValue = 0;
        // * 需要在Dispatcher中更新
        public double TaskValue {
            get => _taskValue;
            set { 
                _taskValue = value;
                RaisePropertyChanged(nameof(TaskValue));
            }
        }
        public bool CheckDownloadPartFinish() {
            return !(string.IsNullOrEmpty(AudioTmpPath) || string.IsNullOrEmpty(VideoTmpPath));
        }
        public ICommand SPButtonCommand => new ReplyCommand<object>(
            (_) => {
                CoreManager.logger.Info(FileName);
            }, true);
        public ICommand CancelButtonCommand => new ReplyCommand<object>(
            (_) => {
                //PageManager.DownloadTaskPage.Dispatcher.Invoke(() => {
                //    TaskValue = new Random().Next(100);
                //});
                CoreManager.logger.Info(VideoDTaskValue.ToString());
                CoreManager.logger.Info(AudioDTaskValue.ToString());
            }, true);
    }
}
