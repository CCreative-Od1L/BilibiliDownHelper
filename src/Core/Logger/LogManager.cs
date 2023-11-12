using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Logger {
    static public partial class LogManager {
        static readonly ConcurrentQueue<Tuple<string, string>> logMessageQueue = new();
        static AutoResetEvent Pause => new(false);
        public static event Action<LogInfo>? OnLogAction;  // * 可自定义Log事件，会在日志记录发生时触发。

        static LogManager() {
            Task logTask = new(obj => {
                while(true) {
                    Pause.WaitOne(2000, true);
                    Dictionary<string, string> logMessageBuf = new();
                    foreach (var logItem in logMessageQueue) {
                        string logPath = logItem.Item1;
                        string mergeMessage = string.Concat(
                            logItem.Item2,
                            Environment.NewLine,
                            "===========================================",
                            Environment.NewLine);
                        bool addResult = logMessageBuf.TryAdd(logPath, mergeMessage);
                        if (!addResult){
                            logMessageBuf[logPath] = string.Concat(
                                logMessageBuf[logPath],
                                mergeMessage);  // * 合并同一文件路径下的Log信息
                        }
                        _ = logMessageQueue.TryDequeue(out _);  // * 处理完成后的消息出队列。
                    }

                    foreach (var logMessage in logMessageBuf) {
                        WriteDown(logMessage.Key, logMessage.Value);
                    }
                }
            }, null, TaskCreationOptions.LongRunning);
            
            _logDirectory = AppDomain.CurrentDomain.BaseDirectory + @"AppData\Logs\";

            logTask.Start();
        }

        static void WriteDown(string logPath, string content) {
            try {
                if (!File.Exists(logPath)) {
                    File.CreateText(logPath).Close();
                }
                using var systemWrite = File.AppendText(logPath);
                systemWrite.Write(content);
            } catch (Exception) {
                // * 忽略此错误
                Console.WriteLine("Logger Error Happen./日志输出到文件出现错误");
            }
        }

        // * 日志文件存放目录
        static string _logDirectory;
        public static string LogDirectory {
            get => _logDirectory;
            set {
                if (value == null) { 
                    Debug.WriteLine("应该输入正确的文件夹路径。");
                    return; 
                }

                if (!Directory.Exists(value)) {
                    Directory.CreateDirectory(value);
                }
                _logDirectory = value;
            }
        }

        [GeneratedRegex("(?<=\\()(\\d+)(?=\\))")]
        private static partial Regex LogNoRegex();
        static string GetLogFilePath() {
            string newFilePath = string.Empty;
            // * 施工中...
            string? logFolderPath = LogDirectory;
            StringBuilder stringBuilder = new();

            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileExtension  = ".log";
            // TODO （整合）构造文件名
            stringBuilder.Append(currentDate);
            stringBuilder.Append("(*)");
            stringBuilder.Append(fileExtension);
            string fileNamePattern = stringBuilder.ToString();
            stringBuilder.Clear();
            List<string> filePaths = Directory.GetFiles(logFolderPath, fileNamePattern, SearchOption.TopDirectoryOnly).ToList();

            if (filePaths.Count > 0) {
                int fileNameMaxLen = filePaths.Max(path => path.Length);
                var latestLogFilePath = filePaths.Where(path => path.Length == fileNameMaxLen).OrderDescending().First();
                if (new FileInfo(latestLogFilePath).Length > 1 * 1024 * 1024) {
                    var strLogNo = LogNoRegex().Match(Path.GetFileName(latestLogFilePath)).Value;
                    var parse = int.TryParse(strLogNo, out int intLogNo);
                    var logFileNo = $"({(parse ? intLogNo + 1 : intLogNo)})";
                    
                    // TODO （整合）构造文件名
                    stringBuilder.Append(currentDate);
                    stringBuilder.Append(logFileNo);
                    stringBuilder.Append(fileExtension);
                    newFilePath = Path.Combine(logFolderPath, stringBuilder.ToString());
                } else {
                    newFilePath = latestLogFilePath;
                }
            } else {
                // TODO （整合）构造文件名
                stringBuilder.Append(currentDate);
                stringBuilder.Append("(0)");
                stringBuilder.Append(fileExtension);
                newFilePath = Path.Combine(logFolderPath, stringBuilder.ToString());
            }
            // * 最后的清理工作
            stringBuilder.Clear();
            return newFilePath;
        }


    }
}