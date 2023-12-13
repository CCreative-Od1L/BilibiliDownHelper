using System.Globalization;
using Core;
using Core.Utils;
using Xunit.Abstractions;

namespace Core.Test {
    public class DownloadHandleTest(ITestOutputHelper testOutputHelper)
    {
        readonly ITestOutputHelper output = testOutputHelper;

        [Fact]
        public void IsDownloadWorkWell() {
            CoreManager.logger.Info("Main", "Logger start");
            output.WriteLine("System start");
            
            

            Thread.Sleep(5000);
            output.WriteLine("System End");
        }
    }
}