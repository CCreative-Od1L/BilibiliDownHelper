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
    public class DownloadTaskEntry : NotificationObject {
        public string Gid { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string TaskRunMessage { get; set; } = "...";
        private double _taskValue = new Random().Next(100);
        // * 需要在Dispatcher中更新
        public double TaskValue {
            get => _taskValue;
            set { 
                _taskValue = value;
                RaisePropertyChanged(nameof(TaskValue));
            }
        }
        public ICommand SPButtonCommand => new ReplyCommand<object>(
            (_) => {
                CoreManager.logger.Info(FileName);
            }, true);
        public ICommand CancelButtonCommand => new ReplyCommand<object>(
            (_) => {
                PageManager.DownloadTaskPage.Dispatcher.Invoke(() => {
                    TaskValue = new Random().Next(100);
                });
                CoreManager.logger.Info(TaskValue.ToString());
            }, true);
    }
}
