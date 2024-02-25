using BvDownkr.src.Entries;
using BvDownkr.src.Implement;
using BvDownkr.src.Models;
using BvDownkr.src.Services;
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
        private readonly Dictionary<string, DownloadTaskEntry> _taskGidDictionary;
        public DownloadTaskVM() {
            _model = new();
            _taskGidDictionary = [];

            DownloadService.AddTellStatusAction(UpdateTaskProgress);
            DownloadService.INSTANCE.AddCreateDTaskAction(AddTaskItem);
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
        private void UpdateTaskProgress(string gid, long totalLength, long completedLength, long speed) {
            var isFound = _taskGidDictionary.TryGetValue(gid, out var goatTask);
            if (isFound) {
                var progressValue = (totalLength / completedLength) * 100;
                PageManager.DownloadTaskPage.Dispatcher.Invoke(() => {
                    goatTask!.TaskValue = progressValue;
                });
            }
        }
        private void AddTaskItem(string gid, string fileName) {
            var task = new DownloadTaskEntry {
                Gid = gid,
                FileName = fileName
            };
            DownloadTasks.Add(task);
            _taskGidDictionary.TryAdd(gid, task);
        }
    }
}
