namespace Core.Logger {
    /// <summary>
    /// * 日志信息（数据类） 
    /// </summary> 
    public class LogInfo {
        public DateTime Time {get; set;}
        public int ThreadID {get; set;}
        /// <summary>
        /// * LogLevel : 日志等级 
        /// </summary>
        public LOG_LEVEL LogLevel {get; set;}
        public string? Source {get; set;}
        public string? Message {get; set;}
        /// <summary>
        /// * Exception Info 
        /// </summary>
        /// <value>
        /// * ExceptionObj : 异常对象
        /// * ExceptionType : 异常类型
        /// </value>
        public Exception? ExceptionObj {get; set;}
        public string? ExceptionType {get; set;}

        // * Web Info
        public string? RequestUrl {get; set;}
        public string? UserAgent {get; set;}
    }
}