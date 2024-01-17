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
    }
}