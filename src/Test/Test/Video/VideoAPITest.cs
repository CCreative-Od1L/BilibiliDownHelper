using System.Globalization;
using Core;
using Core.BilibiliApi.Video;
using Core.Utils;
using Xunit.Abstractions;

namespace Core.Test {
    public class VideoAPITest(ITestOutputHelper testOutputHelper)
    {
        readonly ITestOutputHelper output = testOutputHelper;
        readonly string testFilePath = Path.Combine(CoreManager.directoryMgr.fileDirectory.Cache, "video_test.txt");
        [Fact]
        public async void GetVideoInfoTest() {
            var(isSuccess, videoInfo) = await VideoAPI.GetVideoBaseInfoFromID("170001", "");
            if (!isSuccess) { Assert.True(false); }
            else {
                FileUtils.AppendText(testFilePath, Environment.NewLine + JsonUtils.SerializeJsonObj(videoInfo));
                Assert.True(true);
            }
        }
        [Fact]
        public async void ParseInputTest() {
            List<string> inputStrs = [
                @"https://www.bilibili.com/video/BV1cW4y1c7KN/?spm_id_from=333.1007.tianma.1-2-2.click&vd_source=b7173c5b63af3b52530d80b38c71619e",
            ];
            foreach(var inputStr in inputStrs) {
                var (isSuccess, avid, bvid) = VideoAPI.ParseInputStr(inputStr);
                Assert.True(isSuccess);
                FileUtils.AppendText(testFilePath, Environment.NewLine + string.Format("avid:{0}, bvid:{1}", avid, bvid));
                
                var (isFirstStageSuccess, content) = await VideoAPI.GetVideoBaseInfoFromID(avid, bvid);
                Assert.True(isFirstStageSuccess);
                FileUtils.AppendText(testFilePath, Environment.NewLine + JsonUtils.SerializeJsonObj(content));

                var cids = content!.GetPagesCid();

                for(int i = 0; i < cids.Count; ++i) {
                    var (isSecondStageSuccess, videoData) = await VideoAPI.GetVideoStreamDataFromID(
                        cids[i].ToString(),
                        avid,
                        bvid
                    );
                    Assert.True(isSecondStageSuccess);

                    var (videoLinks, videoQn) = videoData!.GetDashVideoDownloadLink();
                    for(int j = 0; j < videoQn.Count; ++j) {
                        for(int k = 0; k < videoLinks[j].Count; ++k) {
                            FileUtils.AppendText(
                                testFilePath,
                                Environment.NewLine + string.Format("video:{0}-{1}", videoQn[j], videoLinks[j][k]));
                        }
                    }

                    var (audiolinks, audioQn) = videoData.GetDashAudioDownloadLink();
                    for(int j = 0; j < audioQn.Count; ++j) {
                        for(int k = 0; k < audiolinks[j].Count; ++k) {
                            FileUtils.AppendText(
                                testFilePath,
                                Environment.NewLine + string.Format("audio:{0}-{1}", audioQn[j], audiolinks[j][k]));
                        }
                    }
                }
            }
        }
    }
}