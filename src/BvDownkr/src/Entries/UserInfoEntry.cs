using Core.BilibiliApi.User;
using Core.BilibiliApi.Video.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BvDownkr.src.Entries {
    public class UserInfoEntry {
        public string UID { get; set; } = string.Empty;
        public string Uname { get; set; } = string.Empty;
        public int CurrentLevel { get; set; }
        public int CurrentMin { get; set; }
        public int CurrentExp { get; set; }
        public bool IsSeniorMember { get; set; } = false;
        public bool IsVip { get; set; } = false;
        public VIDEO_QUALITY MaxAccessVideoQn { get; set; } = VIDEO_QUALITY._720P;
        public AUDIO_QUALITY MaxAccessAudioQn { get; set; } = AUDIO_QUALITY._192K;
        public List<string> RemindText { get; set; } = [];
        // * 先占个位置
        public UserInfoEntry() { }
        public void ComputeMaxAccessQn(bool isLogin, int userType) {
            if (!isLogin) { 
                MaxAccessVideoQn = VIDEO_QUALITY._720P;
                MaxAccessAudioQn = AUDIO_QUALITY._192K;
                return;
            }
            switch (userType) {
                // * 普通会员
                case 0:
                    MaxAccessVideoQn = VIDEO_QUALITY._1080P_PLUS;
                    break;
                // * 大会员
                case 1:
                    MaxAccessVideoQn = VIDEO_QUALITY._8K;
                    MaxAccessAudioQn = AUDIO_QUALITY.HI_RES;
                    break;
                default:
                    break;
            }
        }
        public void UpdateRemindText(List<string> listString) {
            for(int i = 0; i < listString.Count; ++i) {
                if (!RemindText.Contains(listString[i])) {
                    RemindText.Add(listString[i]);
                }
            }
        }
        public void RemoveRemindText(List<string> listString) { 
            for(int i = 0; i < listString.Count; ++i) {
                RemindText.Remove(listString[i]);
            }
        }
    }
}
