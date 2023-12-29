using System.Text;
using Xunit.Abstractions;

namespace Core.Test {
    public class FileTest(ITestOutputHelper testOutputHelper)
    {
        readonly ITestOutputHelper output = testOutputHelper;
        [Fact]
        public async void BinaryFileWriteIsWorkWell() {
            string filePath = Path.Combine(CoreManager.directoryMgr.fileDirectory.Cache,"test.bin");
            List<string> datas = [
                "&zXK2DXX5^&bE0YzICZjE!RiUFdIyiy88hGnoT0Avf",
                "HCyVdxfdevLx%ixa6xus1B9ip9SMbk^DNfzhOF9ktFYYnrN1If9#n6#1d2240NaalPmPLJvQUzDh81YB7OGiwZynwkK1R3jSm!2",
                "s!UY^ahE0C*rLWEjQw1ks7fKm",
            ];

            using MemoryStream ms = new();
            for(int i = 0; i < datas.Count; ++i) {
                byte[] bData = Encoding.UTF8.GetBytes(datas[i]);
                byte[] bDataLen = BitConverter.GetBytes(bData.Length);
                ms.Write(bDataLen);
                ms.Write(bData);
            }
            ms.Seek(0, SeekOrigin.Begin);

            AutoResetEvent pause = new(false);
            using (FileStream fs = File.Create(filePath)) {
                await Task.Run(async () => {
                    await ms.CopyToAsync(fs);
                    pause.Set();
                });
                pause.WaitOne();
            }
            ms.Close();

            using (FileStream fs = File.OpenRead(filePath)) {
                BinaryReader binaryReader = new(fs);
                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                long overallLen = binaryReader.BaseStream.Length;
                while(binaryReader.BaseStream.Position < overallLen) {
                    byte[] dataLen = binaryReader.ReadBytes(4);
                    string sourceData = Encoding.UTF8.GetString(binaryReader.ReadBytes(BitConverter.ToInt32(dataLen, 0)));
                    output.WriteLine(sourceData);
                }
            }
        }
    }
}