using BvDownkr.src.Entries;
using Core.BilibiliApi.Video.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src.Models {
    public class VideoDownModel {
        public List<VideoDownInfoEntry> VideoDownInfoEntries { get; set; } = [];
        public Dictionary<long, VideoDownInfoEntry> VideoDownDic { get; set; } = [];
        public List<VideoDownInfoEntry> LoadVBInfo(VideoBaseInfoData vbInfo) {
            List<VideoDownInfoEntry> li = [];
            var pages = vbInfo.GetPagesInfo();
            for(int i = 0; i < pages.Count; ++i) {
                var (cid, no, title, duration) = pages[i];
                VideoDownInfoEntry entry = new() {
                    No = no.ToString(),
                    PageName = title,
                    Duration = duration.ToString(),
                };
                li.Add(entry);
                VideoDownDic.TryAdd(cid, entry);
            }
            return li;
        }
        public void SetDefaultSelect() {
            for(int i = 0; i < VideoDownInfoEntries.Count; ++i) {
                VideoDownInfoEntries[i].VideoQnSelected = 0;
                VideoDownInfoEntries[i].AudioQnSelected = 0;
            }
        }
        public void Clean() {
            VideoDownDic.Clear();
        }
    }
}
