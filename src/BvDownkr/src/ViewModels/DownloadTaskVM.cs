using BvDownkr.src.Entries;
using BvDownkr.src.Implement;
using BvDownkr.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BvDownkr.src.ViewModels
{
    public class DownloadTaskVM : NotificationObject {
        private readonly DownloadTaskModel _model;
        public DownloadTaskVM() {
            _model = new();
        }
        public List<DownloadTaskEntry> DownloadTasks {
            get => _model.TasksEntries;
            set {
                _model.TasksEntries = value;
                RaisePropertyChanged(nameof(DownloadTasks));
            }
        }
        public ICommand OnPageLoaded => new ReplyCommand<object>(
            (_) => {
                DownloadTasks.Add(
                    new() {
                        Gid = "abc",
                        FileName = "《艾尔登法环 黄金树幽影》首支宣传视频"
                    });
            }, true);
    }
}
