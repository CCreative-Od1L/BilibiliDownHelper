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
            VideoService.INSTANCE.AddAfterGetVLinkAction(LoadVideoInfo);
            VideoService.INSTANCE.AddAfterGetALinkAction(LoadAudioInfo);
            VideoService.INSTANCE.AddAfterGetDLinksAction(AfterUpdateDownloadLinks);
        }
        public List<VideoDownInfoEntry> VideoDownInfoEntries {
            get => _model.VideoDownInfoEntries;
            set {
                _model.VideoDownInfoEntries = value;
                RaisePropertyChanged(nameof(VideoDownInfoEntries));
            }
        }
        public void CleanUI() {
            VideoDownInfoEntries.Clear();
            _model.Clean();
        }
        private void UpdateListUI(VideoBaseInfoData vbInfo) {
            VideoDownInfoEntries = _model.LoadVBInfo(vbInfo);
        }
        private void LoadVideoInfo(long cid, List<List<string>> links, List<VIDEO_QUALITY> qnList) {
            var isFound = _model.VideoDownDic.TryGetValue(cid, out var entry);
            if (!isFound) { return; }

            for(int i = 0; i < qnList.Count; ++i) {
                entry!.VideoQnList.Add(qnList[i].ToString());
                entry!.VideoDownLinks = links;
            }
        }
        private void LoadAudioInfo(long cid, List<List<string>> links, List<AUDIO_QUALITY> qnList) {
            var isFound = _model.VideoDownDic.TryGetValue(cid, out var entry);
            if (!isFound) { return; }

            for (int i = 0; i < qnList.Count; ++i) {
                entry!.AudioQnList.Add(qnList[i].ToString());
                entry!.AudioDownLinks = links;
            }
        }
        private void AfterUpdateDownloadLinks() {
            _model.SetDefaultSelect();
        }
    }
}
