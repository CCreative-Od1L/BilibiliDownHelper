namespace Core.Utils {
    public class FileUtils {
        static void CheckFile(string path) {
            if (!File.Exists(path)) {
                File.CreateText(path).Close();
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
                CheckFile(filePath);
                using var systemWrite = File.AppendText(filePath);
                systemWrite.Write(content);
            } catch (Exception e) {
                exceptionCallback(e);
            }
        }
        static public void WriteText(string filePath, string content, Action<Exception>? exceptionCallback = null) {
            try {
                CheckFile(filePath);
                File.WriteAllText(filePath, content);
            } catch (Exception e) {
                exceptionCallback?.Invoke(e);
            }
        }
        static public void WriteTextThenEncryptToBytes(string filePath, string content, Action<Exception>? exceptionCallback = null) {
            try {
                // CheckFile(filePath);
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
                CheckFile(filePath);
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