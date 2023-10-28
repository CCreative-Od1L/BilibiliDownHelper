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
                    List<string[]> logMessageBuf = new();
                    foreach (var logItem in logMessageQueue) {
                        
                    }
                }

            }, null, TaskCreationOptions.LongRunning);
        }
    }
}