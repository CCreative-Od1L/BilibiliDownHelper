namespace Core.Utils {
    public class FileUtils {
        static void CheckAndCreateDirectory(string filePath) {
            if (!Directory.Exists(Path.GetDirectoryName(filePath))) {
                var directoryPath = Path.GetDirectoryName(filePath);
                if (directoryPath != null) {
                    Directory.CreateDirectory(directoryPath);
                }
            }
        }
        static public string ReadTextFile(string filePath) {
            if (!File.Exists(filePath)) {
                return string.Empty;
            }
            return File.ReadAllText(filePath);
        }
        static public void AppendText(string filePath, string content, Action<Exception> exceptionCallback) {
            try {
                CheckAndCreateDirectory(filePath);
                using var systemWrite = File.AppendText(filePath);
                systemWrite.Write(content);
            } catch (Exception e) {
                exceptionCallback(e);
            }
        }
        static public void WriteText(string filePath, string content, Action<Exception>? exceptionCallback = null) {
            try {
                CheckAndCreateDirectory(filePath);
                File.WriteAllText(filePath, content);
            } catch (Exception e) {
                exceptionCallback?.Invoke(e);
            }
        }
        static public void WriteTextThenEncryptToBytes(string filePath, string content, Action<Exception>? exceptionCallback = null) {
            try {
                CheckAndCreateDirectory(filePath);
                WriteBytes(filePath, CryptoUtils.AesEncryptStringToBytes(content));
            } catch (Exception e) {
                exceptionCallback?.Invoke(e);
            }
        }
        static public byte[] ReadBytesFile(string filePath) {
            if (!File.Exists(filePath)) {
                return [];
            }
            return File.ReadAllBytes(filePath);
        }
        static public void WriteBytes(string filePath, byte[] content, Action<Exception>? exceptionCallback = null) {
            try {
                CheckAndCreateDirectory(filePath);
                File.WriteAllBytes(filePath, content);
            } catch (Exception e) {
                exceptionCallback?.Invoke(e);
            }
        }
        static public string ReadBytesThenDecryptToText(string filePath) {
            if (!File.Exists(filePath)) {
                return string.Empty;
            }
            var bytes = File.ReadAllBytes(filePath);
            return CryptoUtils.AesDecryptBytesToString(bytes);
        }
    }
}