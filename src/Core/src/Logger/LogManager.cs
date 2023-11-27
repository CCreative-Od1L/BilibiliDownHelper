using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;
using Core.Directory;
using Core.Utils;

namespace Core.Logger {
    static public partial class LogManager {
        /// <summary>
        /// * 存放日志任务。
        /// item1: 日志文件名称
        /// item2: 
        /// </summary> <summary>
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
                            "=================================================================================",
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
                        FileUtils.AppendText(logMessage.Key, logMessage.Value, (e) => {
                            Console.WriteLine("Logger Error Happen./日志输出到文件出现错误");
                        });
                    }
                }
            }, null, TaskCreationOptions.LongRunning);
            
            // ! Must latest call
            CheckAndCreateLogDirectory();
            logTask.Start();
        }

        public static bool CheckAndCreateLogDirectory() {
            if (string.IsNullOrEmpty(DirectoryManager.GetLogDirectory())) {
                DirectoryManager.ResetToDefault("log");

                if (!System.IO.Directory.Exists(DirectoryManager.GetLogDirectory())) {
                    System.IO.Directory.CreateDirectory(DirectoryManager.GetLogDirectory());
                } 
            }
            return true;
        }
        
        [GeneratedRegex("(?<=\\()(\\d+)(?=\\))")]
        private static partial Regex LogNoRegex();
        /// <summary>
        /// 返回构造出来的日志文件名称，格式:yyyyMMdd([pattern]).log
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns> <summary>
        static string ConstructLogFileName(string pattern) {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendFormat("{0}({1}).log", DateTime.Now.ToString("yyyyMMdd"), pattern);
            return stringBuilder.ToString();
        }
        static string GetLogFilePath() {
            string newFilePath = string.Empty;
            CheckAndCreateLogDirectory();
            string logFolderPath = DirectoryManager.GetLogDirectory();
            string fileNamePattern = ConstructLogFileName("*");
            List<string> filePaths = System.IO.Directory.GetFiles(
                logFolderPath, 
                fileNamePattern, 
                SearchOption.TopDirectoryOnly)
                .ToList();
            if (filePaths.Count > 0) {
                int fileNameMaxLen = filePaths.Max(path => path.Length);
                var latestLogFilePath = filePaths.Where(path => path.Length == fileNameMaxLen).OrderDescending().First();
                if (new FileInfo(latestLogFilePath).Length > 1 * 1024 * 1024) {
                    var strLogNo = LogNoRegex().Match(Path.GetFileName(latestLogFilePath)).Value;
                    var parse = int.TryParse(strLogNo, out int intLogNo);
                    var logFileNo = $"{(parse ? intLogNo + 1 : intLogNo)}";
                    newFilePath = Path.Combine(logFolderPath, ConstructLogFileName(logFileNo));
                } else {
                    newFilePath = latestLogFilePath;
                }
            } else {
                newFilePath = Path.Combine(logFolderPath, ConstructLogFileName("0"));
            }
            return newFilePath;
        }
        static void pushBackLogMessageQueue(string message) {
            logMessageQueue.Enqueue(new Tuple<string, string>(
                GetLogFilePath(),
                message
            ));
        }
        #region Level: Info
        public static void Info(string info) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.INFO,
                Message = info,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Info(string source, string info) {
            var logInfo = new LogInfo(){
                LogLevel = LOG_LEVEL.INFO,
                Message = info,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source                
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Info(Type source, string info) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.INFO,
                Message = info,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source.FullName
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        #endregion

        #region Level: Debug
        public static void Debug(string info) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.DEBUG,
                Message = info,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Debug(string source, string info) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.DEBUG,
                Message = info,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Debug(Type source, string info) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.DEBUG,
                Message = info,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source.FullName,
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        #endregion
        
        #region Level: Error
        public static void Error(Exception error) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = error.Message,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = error.Source,
                ExceptionObj = error,
                ExceptionType = error.GetType().Name
            };
            pushBackLogMessageQueue(logInfo.ToString()) ;
            OnLogAction?.Invoke(logInfo);
        }
        public static void Error(string source, Exception error) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = error.Message,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source,
                ExceptionObj = error,
                ExceptionType = error.GetType().Name
            };
            pushBackLogMessageQueue(logInfo.ToString()) ;
            OnLogAction?.Invoke(logInfo);
        }
        public static void Error(Type source, Exception error) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = error.Message,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source.FullName,
                ExceptionObj = error,
                ExceptionType = error.GetType().FullName
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Error(string source, string error) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = error,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source,
                ExceptionType = error.GetType().FullName
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Error(Type source, string error) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = error,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                ExceptionObj = null,
                ExceptionType = error.GetType().Name
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        #endregion

        #region Level: Fatal
        public static void Fatal(Exception fatal) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = fatal.Message,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                ExceptionObj = fatal,
                ExceptionType = fatal.GetType().Name
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Fatal(string source, Exception fatal) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = fatal.Message,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source,
                ExceptionObj = fatal,
                ExceptionType = fatal.GetType().Name
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Fatal(Type source, Exception fatal) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = fatal.Message,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source.FullName,
                ExceptionObj = fatal,
                ExceptionType = fatal.GetType().Name
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Fatal(string source, string fatal) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = fatal,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source,
                ExceptionType = fatal.GetType().Name
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        public static void Fatal(Type source, string fatal) {
            var logInfo = new LogInfo() {
                LogLevel = LOG_LEVEL.ERROR,
                Message = fatal,
                Time = DateTime.Now,
                ThreadID = Environment.CurrentManagedThreadId,
                Source = source.FullName,
                ExceptionType = fatal.GetType().Name
            };
            pushBackLogMessageQueue(logInfo.ToString());
            OnLogAction?.Invoke(logInfo);
        }
        #endregion
    }
}