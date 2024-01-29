using System.Globalization;
using Core;
using Core.BilibiliApi.Video;
using Core.FFmpegFunc;
using Core.Utils;
using Xunit.Abstractions;

namespace Core.Test {
    public class FFmpegAPITest(ITestOutputHelper testOutputHelper)
    {
        readonly ITestOutputHelper output = testOutputHelper;
        readonly string testFilePath = Path.Combine(CoreManager.directoryMgr.fileDirectory.Cache, "ffmpeg_test.txt");
        [Fact]
        public async void MixAudioTest() {
            string outputPath = Path.Combine(CoreManager.directoryMgr.fileDirectory.Cache, "output.mp4");

            string videoPath = Path.Combine(CoreManager.directoryMgr.fileDirectory.Cache, "1418545436-1-100113.m4s");
            string audioPath = Path.Combine(CoreManager.directoryMgr.fileDirectory.Cache, "1418545436-1-30280.m4s");

            // var (changeVideoFormatResult, tmpVideoPath) = await FFmpegAPI.ChangeVideoFormat(videoPath, "mp4", Xabe.FFmpeg.VideoCodec.h264);
            // var (changeAudioFormatResult, tmpAudioPath) = await FFmpegAPI.ChangeAudioFormat(audioPath, "mp3", Xabe.FFmpeg.AudioCodec.aac);

            var result = await FFmpegAPI.MixAudio(outputPath, videoPath, audioPath);

            FileUtils.AppendText(
                Path.Combine(CoreManager.directoryMgr.fileDirectory.Cache, testFilePath),
                string.Format("startTime:{0}\nendTime:{1}\nduration:{2}\narguments:{3}\n",result.StartTime, result.EndTime, result.Duration, result.Arguments)
            );
        }
    }
}