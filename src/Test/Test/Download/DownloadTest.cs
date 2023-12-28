using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using Core;
using Core.Aria2cNet.Server;
using Core.Utils;
using Xunit.Abstractions;

namespace Core.Test {
    public class DownloadHandleTest(ITestOutputHelper testOutputHelper)
    {
        readonly ITestOutputHelper output = testOutputHelper;
        [Fact]
        public void IsMainWorkWell() {
            CoreManager.logger.Info("Main", "Logger start");
            output.WriteLine("System start");
            
            AutoResetEvent done = new(false);
            BaseDownloadTest(done);
            
            done.WaitOne();

            Thread.Sleep(5000);
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
            var openServerResult = await ServerSingleton.Instance.AsyncStartServer(
                new Aria2Config(),
                output.WriteLine
            );
            Assert.True(openServerResult);

            var closeResult = await ServerSingleton.Instance.CloseServerAsync();
            Assert.True(closeResult);

            done.Set();
        }
    }
}