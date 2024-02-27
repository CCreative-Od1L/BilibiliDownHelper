using BvDownkr.src.Entries;
using BvDownkr.src.Implement;
using BvDownkr.src.Models;
using BvDownkr.src.Services;
using Core.BilibiliApi.Video.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src.ViewModels {
    public class VideoDownPageVM : NotificationObject {
        private readonly VideoDownModel _model;
        public VideoDownPageVM() {
            _model = new();

            VideoService.INSTANCE.AddAfterGetVBInfoAction(UpdateListUI);
        }
        public List<VideoDownInfoEntry> VideoDownInfoEntries {
            get => _model.VideoDownInfoEntries;
            set {
                _model.VideoDownInfoEntries = value;
                RaisePropertyChanged(nameof(VideoDownInfoEntries));
            }
        }
        private void UpdateListUI(VideoBaseInfoData vbInfo) {
            VideoDownInfoEntries = VideoDownModel.LoadVBInfo(vbInfo);
        }
        private void LoadDownloadInfo() {

        }
    }
}
