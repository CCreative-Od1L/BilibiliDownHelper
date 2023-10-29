using System.Collections.Concurrent;

namespace Core.Logger {
    static public class LogManager {
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
        static string _logDirectory = AppDomain.CurrentDomain.BaseDirectory + @"AppData\Logs\";
        public static string LogDirectory {
            get => _logDirectory;
            set {
                if (!Directory.Exists(value)) {
                    Directory.CreateDirectory(value);
                }
                _logDirectory = value;
            }
        }

        static string GetLogFilePath() {
            string newFilePath = string.Empty;
            // * 施工中...
            return newFilePath;
        }
    }
}