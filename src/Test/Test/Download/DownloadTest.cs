using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using Core;
using Core.Aria2cNet;
using Core.Aria2cNet.Client;
using Core.Aria2cNet.Client.Entity;
using Core.Aria2cNet.Server;
using Core.Utils;
using Xunit.Abstractions;

namespace Core.Test {
    public class DownloadHandleTest(ITestOutputHelper testOutputHelper)
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

        public async void BaseDownloadTest(AutoResetEvent done) {
            var option = new Aria2Config();
            option.AddBilibiliReferer();

            AutoResetEvent downloadStop = new(false);

            AriaManager.Instance.DownloadFinish += 
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

            string downloadFileName = "download_test.dat";
            var downTask = await ClientSingleton.Instance.AddUriAsync(
                [
                    "https://i2.hdslb.com/bfs/face/399f5b1fc45eb24c4b812a081e5441ffce5af3b8.jpg"
                ],
                new AriaSendOption() {  
                    Dir = CoreManager.directoryMgr.fileDirectory.Cache,
                    Out = downloadFileName,
                }
            );

            if (downTask != null) {
                string gid = downTask.Result;
                Assert.NotEmpty(gid);

                _ = Task.Run(() => {
                    _ = AriaManager.Instance.AsyncGetDownloadStatus(gid, downloadStop);
                });
            }

            downloadStop.WaitOne();
            
            var closeResult = await ServerSingleton.Instance.CloseServerAsync();
            Assert.True(closeResult);

            done.Set();
        }
    }
}