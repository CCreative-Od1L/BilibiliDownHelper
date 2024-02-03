using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Core;
using Core.Aria2cNet;
using Core.Aria2cNet.Client;
using Core.Aria2cNet.Client.Entity;
using Core.Aria2cNet.Server;
using Core.Utils;
using Xunit.Abstractions;

namespace Core.Test {
    public partial class DownloadHandleTest(ITestOutputHelper testOutputHelper)
    {
        readonly ITestOutputHelper output = testOutputHelper;
        readonly string downloadLogPath = Path.Combine(CoreManager.directoryMgr.fileDirectory.Cache, "download_test.txt");
        [Fact]
        public void IsMainWorkWell() {
            CoreManager.logger.Info("Main", "Logger start");
            output.WriteLine("System start");
            
            AutoResetEvent done = new(false);
            BaseDownloadTest(done);
            
            done.WaitOne();

            output.WriteLine("System End");
        }
        // version 1.0
        public void IsZipWorkWell() {
            // 64bits version
            // TODO ariaZipPath 程序化 分32位 & 64位
            // TODO 通过正则表达式来识别 aria2 的压缩包文件
            string ariaZipPath = @"third_party/aria2-1.37.0-win-64bit-build1.zip";
            string extraPath = AppDomain.CurrentDomain.BaseDirectory;
            using ZipArchive archive = ZipFile.OpenRead(ariaZipPath);
            foreach (ZipArchiveEntry entry in archive.Entries) {
                if (entry.FullName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) {
                    string destinationPath = Path.GetFullPath(Path.Combine(extraPath, entry.Name));
                    if (destinationPath.StartsWith(extraPath, StringComparison.Ordinal)) {
                        entry.ExtractToFile(destinationPath);
                    }
                    output.WriteLine("Extract File Name: " + entry.FullName);
                }
            }
        }

        [GeneratedRegex(@"(?<=/)[^/?#]+(?=[^/]*$)gm")]
        private partial Regex UrlFileNameRegex();

        public async void BaseDownloadTest(AutoResetEvent done) {
            var option = new Aria2Config();
            option.AddBilibiliReferer();

            AutoResetEvent downloadStop = new(false);

            AriaManager.INSTANCE.DownloadFinish += 
            (string gid, bool isSuccess, string downloadPath, string? msg = null) => {
                FileUtils.AppendText(
                    downloadLogPath,
                    Environment.NewLine +
                    string.Format("gid:{0} isSuccess:{1} downloadPath:{2} msg:{3}", gid, isSuccess.ToString(), downloadPath, msg)
                );
            };

            var openServerResult = await ServerSingleton.Instance.AsyncStartServer(
                option,
                output.WriteLine
            );
            Assert.True(openServerResult);

            
            string downloadUrl = @"https://upos-sz-mirror08c.bilivideo.com/upgcxcode/36/54/1418545436/1418545436-1-100113.m4s?e=ig8euxZM2rNcNbdlhoNvNC8BqJIzNbfqXBvEqxTEto8BTrNvN0GvT90W5JZMkX_YN0MvXg8gNEV4NC8xNEV4N03eN0B5tZlqNxTEto8BTrNvNeZVuJ10Kj_g2UB02J0mN0B5tZlqNCNEto8BTrNvNC7MTX502C8f2jmMQJ6mqF2fka1mqx6gqj0eN0B599M=&uipk=5&nbs=1&deadline=1706533948&gen=playurlv2&os=08cbv&oi=3071518783&trid=ddb4e9c4093649e6b72dde4711021b20u&mid=1763210270&platform=pc&upsig=f28bbaba4267b539010f64baaf4bb04d&uparams=e,uipk,nbs,deadline,gen,os,oi,trid,mid,platform&bvc=vod&nettype=0&orderid=0,3&buvid=&build=0&f=u_0_0&agrr=0&bw=111681&logo=80000000";
            string downloadFileName = UrlFileNameRegex().Match(downloadUrl).Value;
            var downTask = await ClientSingleton.Instance.AddUriAsync(
                [downloadUrl],
                new AriaSendOption() {  
                    Dir = CoreManager.directoryMgr.fileDirectory.Cache,
                    Out = downloadFileName,
                }
            );

            if (downTask != null) {
                string gid = downTask.Result;
                Assert.NotEmpty(gid);

                _ = Task.Run(() => {
                    _ = AriaManager.INSTANCE.AsyncGetDownloadStatus(gid, downloadStop);
                });
            }

            downloadStop.WaitOne();
            
            var closeResult = await ServerSingleton.Instance.CloseServerAsync();
            Assert.True(closeResult);

            done.Set();
        }

        [Fact]
        public void GetFileNameFromUrl() {
            string pattern = @"(?<=\/)[^\/\?#]+(?=[^\/]*$)";
            string val = "https://i2.hdslb.com/bfs/face/399f5b1fc45eb24c4b812a081e5441ffce5af3b8.jpg";
            var matchResult = Regex.Match(val, pattern);
            Assert.NotEmpty(matchResult.Value);
        }
    }
}