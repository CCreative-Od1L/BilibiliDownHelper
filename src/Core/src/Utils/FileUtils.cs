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
        /// * 检查文件是否存在
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
        static public byte[] ReadBytesFile(string filePath) {
            if (!File.Exists(filePath)) {
                CoreManager.logger?.Info(string.Format("文件 {0} 不存在", Path.GetFileName(filePath)));
                return [];
            }
            return File.ReadAllBytes(filePath);
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
                File.WriteAllBytes(filePath, content);
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
            var bytes = File.ReadAllBytes(filePath);
            return CryptoUtils.AesDecryptBytesToString(bytes);
        }
    }
}