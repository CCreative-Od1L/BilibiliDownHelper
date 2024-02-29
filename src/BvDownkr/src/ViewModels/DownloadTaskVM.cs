using BvDownkr.src.Entries;
using BvDownkr.src.Implement;
using BvDownkr.src.Models;
using BvDownkr.src.Services;
using Core.FFmpegFunc;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BvDownkr.src.ViewModels
{
    public class DownloadTaskVM : NotificationObject {
        private readonly DownloadTaskModel _model;
        private readonly Dictionary<string, BilibiliDownloadTaskEntry> _taskGidDictionary;
        public DownloadTaskVM() {
            _model = new();
            _taskGidDictionary = [];

            DownloadService.AddTellStatusAction(UpdateDownloadTaskProgress);
            DownloadService.INSTANCE.AddCreateDTaskAction(AddTaskItem);
            DownloadService.AddDownloadFinishAction(OnDownloadFinish);
        }
        public List<BilibiliDownloadTaskEntry> DownloadTasks {
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
                        VGid = "abc",
                        AGid = "abc",
                        FileName = "《艾尔登法环 黄金树幽影》首支宣传视频"
                    });
            }, true);
        private void UpdateDownloadTaskProgress(string gid, long totalLength, long completedLength, long speed) {
            var isFound = _taskGidDictionary.TryGetValue(gid, out var goatTask);
            if (isFound) {
                if (completedLength == 0) { return; }
                var progressValue = (totalLength / completedLength) * 100;
                if (gid.Equals(goatTask!.VGid)) {
                    PageManager.DownloadTaskPage.Dispatcher.Invoke(() => {
                        goatTask!.VideoDTaskValue = progressValue;
                    });
                } else {
                    PageManager.DownloadTaskPage.Dispatcher.Invoke(() => {
                        goatTask!.AudioDTaskValue = progressValue;
                    });
                }
            }
        }
        private void OnDownloadFinish(string gid, bool isSuccess, string downloadPath, string? msg = null) {
            var isFound = _taskGidDictionary.TryGetValue(gid, out var goatTask);
            if (isFound && isSuccess) {
                if (gid.Equals(goatTask!.VGid)) {
                    goatTask.VideoTmpPath = downloadPath;
                    PageManager.DownloadTaskPage.Dispatcher.Invoke(() => {
                        goatTask!.VideoDTaskValue = 100;
                    });
                } else {
                    goatTask.AudioTmpPath = downloadPath;
                    PageManager.DownloadTaskPage.Dispatcher.Invoke(() => {
                        goatTask!.AudioDTaskValue = 100;
                    });
                }

                if (goatTask.CheckDownloadPartFinish()) {
                    Task mixTask = new(async () => {
                        var tmpOutputPath = Path.Combine(goatTask.SaveFileDirPath, "output.mp4");
                        _ = await FFmpegAPI.MixAudio(
                            output: tmpOutputPath,
                            videoFilePath: goatTask.VideoTmpPath,
                            audioFilePath: goatTask.AudioTmpPath
                            );
                        FileUtils.RenameFile(tmpOutputPath, goatTask.FileName);
                        FileUtils.RemoveFile([goatTask.VideoTmpPath, goatTask.AudioTmpPath]);
                        CleanTask(goatTask);
                    }, TaskCreationOptions.None);
                    mixTask.Start();
                }
            }
        }
        private void AddTaskItem(string Vgid, string Agid, string fileDir, string fileName) {
            var task = new BilibiliDownloadTaskEntry {
                VGid = Vgid,
                AGid = Agid,
                SaveFileDirPath = fileDir,
                FileName = fileName
            };
            DownloadTasks.Add(task);
            RaisePropertyChanged(nameof(DownloadTasks));

            _taskGidDictionary.TryAdd(Vgid, task);
            _taskGidDictionary.TryAdd(Agid, task);
        }
        public void CleanTask(BilibiliDownloadTaskEntry entry) {
            _taskGidDictionary.Remove(entry.VGid);
            _taskGidDictionary.Remove(entry.AGid);
            DownloadTasks.Remove(entry);
        }
    }
}
