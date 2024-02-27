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
        public static List<VideoDownInfoEntry> LoadVBInfo(VideoBaseInfoData vbInfo) {
            List<VideoDownInfoEntry> li = [];
            var pages = vbInfo.GetPagesInfo();
            for(int i = 0; i < pages.Count; ++i) {
                var (no, title, duration) = pages[i];
                li.Add(new() {
                    No = no.ToString(),
                    PageName = title,
                    Duration = duration.ToString(),
                });
            }
            return li;
        }
    }
}
