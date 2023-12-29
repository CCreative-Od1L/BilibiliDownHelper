using System.Data.SqlTypes;
using System.Text;

namespace Core.Utils {
    // ! 因为 FileUtils 会在CoreManager初始化完成前被调用，所以 logger 是可能为空的。
    // ! 所以要注意在 FileUtils 进行日志记录的时候，需要小心再小心。
    public class FileUtils {
        /// <summary>
        /// * 检查文件夹是否存在
        /// </summary>
        /// <param name="directoryPath"></param>
        static public void CheckAndCreateDirectory(string directoryPath) {
            if (!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }
            CoreManager.logger?.Info(string.Format("创建文件夹 {0}", directoryPath));
        }
        /// <summary>
        /// * 检查文件的文件夹是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        static public void CheckAndCreateFileDirectory(string filePath) {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath)) {
                if (directoryPath != null) {
                    Directory.CreateDirectory(directoryPath);
                }
            }
            CoreManager.logger?.Info(string.Format("创建文件 {0} 的文件夹 {1}", Path.GetFileName(filePath), Path.GetDirectoryName(filePath)));
        }
        /// <summary>
        /// * 创建空文件
        /// ! 会导致文件路径上的文件被复写为空文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        static public void CreateEmptyFile(string filePath) {
            CheckAndCreateFileDirectory(filePath);
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
            using var fs = File.Create(filePath);
            CoreManager.logger?.Info(string.Format("创建空文件 {0}", Path.GetFileName(filePath)));
        }
        /// <summary>
        ///  * 尝试创建文件
        ///  * 如果文件存在则不做操作
        /// </summary>
        /// <param name="filePath"></param>
        static public void TryToCreateFile(string filePath) {
            CheckAndCreateFileDirectory(filePath);
            if (!File.Exists(filePath)) {
                using var fs = File.Create(filePath);
            }
            CoreManager.logger?.Info(string.Format("创建文件 {0}", Path.GetFileName(filePath)));
        }
        /// <summary>
        /// * 读文件
        /// </summary>
        /// <param name="filePath"> 文件路径 </param>
        /// <returns> string 文件经过默认编码后的字符串 </returns>
        static public string ReadTextFile(string filePath) {
            if (!File.Exists(filePath)) {
                CoreManager.logger?.Info(string.Format("文件 {0} 不存在", Path.GetFileName(filePath)));
                return string.Empty;
            }
            return File.ReadAllText(filePath);
        }
        /// <summary>
        /// * 在文件结尾添加文本
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">内容</param>
        /// <param name="exceptionCallback">异常处理回调函数</param>
        static public void AppendText(string filePath, string content, Action<Exception> exceptionCallback) {
            try {
                CheckAndCreateFileDirectory(filePath);
                using var systemWrite = File.AppendText(filePath);
                systemWrite.Write(content);
            } catch (Exception e) {
                CoreManager.logger?.Error(nameof(AppendText), e);
                exceptionCallback?.Invoke(e);
            }
        }
        /// <summary>
        /// * 文件复写
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">内容</param>
        /// <param name="exceptionCallback">异常处理回调函数</param>
        static public void WriteText(string filePath, string content, Action<Exception>? exceptionCallback = null) {
            try {
                CheckAndCreateFileDirectory(filePath);
                File.WriteAllText(filePath, content);
            } catch (Exception e) {
                CoreManager.logger?.Error(nameof(WriteText), e);
                exceptionCallback?.Invoke(e);
            }
        }
        /// <summary>
        /// * 写文件
        /// * 文本 -> 二进制。附带加密
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        static public void WriteTextThenEncryptToBytes(string filePath, string content, Action<Exception>? exceptionCallback = null) {
            try {
                CheckAndCreateFileDirectory(filePath);
                WriteBytes(filePath, CryptoUtils.AesEncryptStringToBytes(content));
            } catch (Exception e) {
                exceptionCallback?.Invoke(e);
            }
        }
        /// <summary>
        /// * 读文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>二进制数据</returns>
        static public byte[] ReadBytesFileDirectly(string filePath) {
            if (!File.Exists(filePath)) {
                CoreManager.logger?.Info(string.Format("文件 {0} 不存在", Path.GetFileName(filePath)));
                return [];
            }
            using var fs = File.OpenRead(filePath);
            BinaryReader binaryReader = new(fs);
            binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
            List<byte> bytes = [];
            long streamLen = binaryReader.BaseStream.Length;
            if (streamLen > int.MaxValue) {
                while(binaryReader.BaseStream.Position < streamLen) {
                    bytes.AddRange(binaryReader.ReadBytes(int.MaxValue));
                }
            } else {
                bytes.AddRange(binaryReader.ReadBytes((int)binaryReader.BaseStream.Length));
            }
            return [.. bytes];
        }
        /// <summary>
        /// * 写文件，写入二进制数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        static public void WriteBytes(string filePath, byte[] content, Action<Exception>? exceptionCallback = null) {
            try {
                CheckAndCreateFileDirectory(filePath);
                using var fs = File.OpenWrite(filePath);
                using MemoryStream ms = new();
                ms.Write(content);
                ms.Position = 0;
                ms.CopyTo(fs);
            } catch (Exception e) {
                CoreManager.logger?.Error(nameof(WriteBytes), e);
                exceptionCallback?.Invoke(e);
            }
        }
        /// <summary>
        /// * 读文件
        /// * 读二进制数据，附带解密 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>经过默认编码后的字符串</returns>
        static public string ReadBytesThenDecryptToText(string filePath) {
            if (!File.Exists(filePath)) {
                return string.Empty;
            }
            using var fs = File.OpenRead(filePath);
            BinaryReader binaryReader = new(fs);
            binaryReader.BaseStream.Position = 0;

            List<byte> bytes = [];
            long streamLen = binaryReader.BaseStream.Length;
            if (streamLen > int.MaxValue) {
                while(binaryReader.BaseStream.Position < streamLen) {
                    bytes.AddRange(binaryReader.ReadBytes(int.MaxValue));
                }
            } else {
                bytes.AddRange(binaryReader.ReadBytes((int)binaryReader.BaseStream.Length));
            }
            return CryptoUtils.AesDecryptBytesToString([.. bytes]);
        }
        /// <summary>
        /// * 异步重写文件
        /// * 以二进制格式存储 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        static public async void AsyncWriteFile(string filePath, List<string> contents) {
            CheckAndCreateFileDirectory(filePath);
            using MemoryStream ms = new();
            for(int i = 0; i < contents.Count; ++i) {
                byte[] bData = Encoding.UTF8.GetBytes(contents[i]);
                byte[] bDataLen = BitConverter.GetBytes(bData.Length);
                if (!BitConverter.IsLittleEndian) {
                    Array.Reverse(bData);
                    Array.Reverse(bDataLen);
                }
                ms.Write(bDataLen);
                ms.Write(bData);
            }
            ms.Seek(0, SeekOrigin.Begin);

            using FileStream fs = File.Create(filePath);
            await ms.CopyToAsync(fs);
        }
        /// <summary>
        /// * 从结尾添加数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="contents"></param>
        static public async void AsyncAppendFile(string filePath, List<string> contents) {
            if (!File.Exists(filePath)) {
                CoreManager.logger.Error(nameof(AsyncAppendFile), new Exception(string.Format("{0} 文件不存在", filePath)));
                return;
            }
            CheckAndCreateFileDirectory(filePath);
            using MemoryStream ms = new();
            for(int i = 0; i < contents.Count; ++i) {
                byte[] bData = Encoding.UTF8.GetBytes(contents[i]);
                byte[] bDataLen = BitConverter.GetBytes(bData.Length);
                if (!BitConverter.IsLittleEndian) {
                    Array.Reverse(bData);
                    Array.Reverse(bDataLen);
                }
                ms.Write(bDataLen);
                ms.Write(bData);
            }
            ms.Seek(0, SeekOrigin.Begin);
            
            using FileStream fs = File.OpenWrite(filePath);
            fs.Seek(0, SeekOrigin.End);
            await ms.CopyToAsync(fs);
        }
        /// <summary>
        /// * 读文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        static public List<string> ReadFile(string filePath, int count = -1) {
            if (!File.Exists(filePath)) { return []; }
            bool isReadAll = count < 0;
            List<string> fileData = [];
            using (FileStream fs = File.OpenRead(filePath)) {
                BinaryReader binaryReader = new(fs);
                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                long overallLen = binaryReader.BaseStream.Length;
                
                while(binaryReader.BaseStream.Position < overallLen) {
                    byte[] dataLen = binaryReader.ReadBytes(4);
                    fileData.Add(
                        Encoding.UTF8.GetString(
                            // * 固定以小端为开始
                            binaryReader.ReadBytes(BitConverter.ToInt32(dataLen, 0))
                        )
                    );
                    if (!isReadAll) {
                        if (--count == 0) { break; }
                    }
                }
            }
            return fileData;
        }
    }
}