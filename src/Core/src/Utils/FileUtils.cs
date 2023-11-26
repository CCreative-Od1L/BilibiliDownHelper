namespace Core.Utils {
    public class FileUtils {
        static void CheckFile(string path) {
            if (!File.Exists(path)) {
                File.CreateText(path).Close();
            }
        }
        static public string ReadFile(string filePath) {
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
        static public void WriteText(string filePath, string content, Action<Exception> exceptionCallback) {
            try {
                CheckFile(filePath);
                File.WriteAllText(filePath, content);
            } catch (Exception e) {
                exceptionCallback(e);
            }
        }
    }
}