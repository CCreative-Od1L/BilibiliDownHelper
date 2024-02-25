﻿using Core;
using Core.BilibiliApi.Video;
using Core.BilibiliApi.Video.Model;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src.Services {
    public class VideoService {
        public static VideoService INSTANCE { get; private set; } = new();
        #region About Events
        public delegate void AfterGetVideoBaseInfoHandler(VideoBaseInfoData videoBaseInfoData);
        private event AfterGetVideoBaseInfoHandler? AfterGetVideoBaseInfo;
        public void AddAfterGetVBInfoAction(AfterGetVideoBaseInfoHandler action) { AfterGetVideoBaseInfo += action; }
        public void RemoveAfterGetVBInfoAction(AfterGetVideoBaseInfoHandler action) { AfterGetVideoBaseInfo -= action; }

        public delegate void AfterGetVideoDownloadLinkHandler(string cid, List<List<string>> links, List<VIDEO_QUALITY> qnList);
        private event AfterGetVideoDownloadLinkHandler? AfterGetVideoDownloadLink;
        public void AddAfterGetVLinkAction(AfterGetVideoDownloadLinkHandler action) { AfterGetVideoDownloadLink += action; }
        public void RemoveAfterGetVLinkAction(AfterGetVideoDownloadLinkHandler action) { AfterGetVideoDownloadLink -= action; }

        public delegate void AfterGetAudioDownloadLinkHandler(string cid, List<List<string>> links, List<AUDIO_QUALITY> qnList);
        private event AfterGetAudioDownloadLinkHandler? AfterGetAudioDownloadLink;
        public void AddAfterGetALinkAction(AfterGetAudioDownloadLinkHandler action) { AfterGetAudioDownloadLink += action; }
        public void RemoveAfterALinkAction(AfterGetAudioDownloadLinkHandler action) { AfterGetAudioDownloadLink -= action; }
        #endregion
        public void ParseUserInput(string inputStr) {
            var (isSuccess, avid, bvid) = VideoAPI.ParseInputStr(inputStr);
            if (!isSuccess) {
                CoreManager.logger.Error(new("用户输入解析失败"));
                return;
            }
            // * 新建线程获取信息
            Task task = new(async () => {
                var (isGetDataSuccess, content) = await VideoAPI.GetVideoBaseInfoFromID(avid, bvid);
                if (isGetDataSuccess) {
                    AfterGetVideoBaseInfo?.Invoke(content!);
                    await GetVideoDownloadLinkAsync(content!, avid, bvid);
                }
            });
            task.Start();
            
        }
        private async Task GetVideoDownloadLinkAsync(VideoBaseInfoData content, string avid, string bvid) {
            var cids = content!.GetPagesCid();
            for (int i = 0; i < cids.Count; ++i) {
                var (isSuccess, videoData) = await VideoAPI.GetVideoStreamDataFromID(
                    cids[i].ToString(),
                    avid,
                    bvid
                );
                if (!isSuccess) return;
                // * 视频相关
                var (videoLinks, videoQn) = videoData!.GetDashVideoDownloadLink();
                AfterGetVideoDownloadLink?.Invoke(cids[i].ToString(), videoLinks, videoQn);
                // * 音频相关
                var (audiolinks, audioQn) = videoData.GetDashAudioDownloadLink();
                AfterGetAudioDownloadLink?.Invoke(cids[i].ToString(), audiolinks, audioQn);
            }
        }
    }
}