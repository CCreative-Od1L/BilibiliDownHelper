using System.Text;
using System.Text.RegularExpressions;

namespace Core.Aria2cNet.Server;
/// <summary>
/// * 日志等级
/// </summary>
public enum Aria2ConfigLogLevel {
    NOT_SET = 0,
    DEBUG = 1,
    INFO,
    NOTICE,
    WARN,
    ERROR,
}
/// <summary>
/// * 文件分配方式
/// </summary>
public enum Aria2ConfigFileAllocation {
    NOT_SET = 0,
    NONE = 1,
    PREALLOC,
    FALLOC,
}
public class Aria2Config {
    public int ListenPort { get; set; }                 // * 服务器端口号
    public string? Token { get; set; }                  // * 链接服务器Token
    public string? LogFilePath { get; set; }            // * 日志文件保存路径
    public Aria2ConfigLogLevel LogLevel { get; set; }  // * 日志等级
    private int _maxConcurrentDownloads;
    public int MaxConcurrentDownloads {                 // * 最大同时下载数量
        get => _maxConcurrentDownloads;
        set {
            _maxConcurrentDownloads = Math.Clamp(value, 1, 20);
        }
    }
    private int _maxConnectionPerServer;
    public int MaxConnectionPerServer {                 // * 同服务器链接数量
        get => _maxConnectionPerServer;
        set {
            _maxConnectionPerServer = Math.Clamp(value, 1, 16);
        }
    }
    private int _split;
    public int Split {                                  // * 单文件最大线程数
        get => _split;
        set {
            _split = Math.Clamp(value, 1, 16); 
        }
    }
    private int _minSplitSize;
    public int MinSplitSize {                           // * 最小分割大小
        get => _minSplitSize;
        set {
            _minSplitSize = Math.Clamp(value, 1, 1024 * 1024);
        }
    }
    private long _maxOverallDownloadLimit;
    public long MaxOverallDownloadLimit {               // * 总下载速度限制
        get => _maxOverallDownloadLimit;
        set {
            _maxOverallDownloadLimit = value > 1 ? value : 1;
        }
    }
    private long _maxDownloadLimit;                     // * 单文件下载速度限制
    public long MaxDownloadLimit { 
        get => _maxDownloadLimit;
        set {
            _maxDownloadLimit = value > 1 ? value : 1;
        }
    }
    private long _maxOverallUploadLimit;                // * 总上传速度限制
    public long MaxOverallUploadLimit { 
        get => _maxOverallUploadLimit;
        set {
            _maxOverallUploadLimit = value > 1 ? value : 1;
        }
    }
    private long _maxUploadLimit;                       // * 单文件上传速度限制
    public long MaxUploadLimit {
        get => _maxUploadLimit;
        set {
            _maxUploadLimit = value > 1 ? value : 1;
        }
    }
    public bool ContinueDownload { get; set; }          // * 断点续传
    public List<string>? Headers { get; set; }
    public Aria2ConfigFileAllocation FileAllocation 
                                 { get; set; }          // * 文件预分配
    private readonly StringBuilder configBuilder;
    public Aria2Config() {
        ListenPort = 6800;
        Token = "bvdownkr";
        MaxConcurrentDownloads = 3;
        MaxConnectionPerServer = 3;
        Split = 5;
        MinSplitSize = 20;
        MaxOverallDownloadLimit = 2048;
        MaxDownloadLimit = 1024;
        MaxOverallUploadLimit = 1024;
        MaxUploadLimit = 256;
        ContinueDownload = true;

        LogLevel = Aria2ConfigLogLevel.NOT_SET;
        FileAllocation = Aria2ConfigFileAllocation.PREALLOC;

        configBuilder = new();
    }
    public void SetLogConfig(string filePath, Aria2ConfigLogLevel level = Aria2ConfigLogLevel.INFO) {
        if (string.IsNullOrEmpty(filePath)) { return; }
        LogFilePath = filePath;
        LogLevel = level;
    }
    private void AppendConfig(string configName, string? configValue = null) {
        if (string.IsNullOrEmpty(configValue)) {
            configBuilder.AppendFormat("--{0} ", configName);
        } else {
            configBuilder.AppendFormat("--{0}={1} ", configName, configValue);
        }
    }
    public override string ToString()
    {
        configBuilder.Clear();
        // * header
        if (Headers != null) {
            for(int i = 0; i < Headers.Count; ++i) {
                AppendConfig("header", $"\"{Headers[i]}\"");
            }
        }
        // * default config
        AppendConfig("enable-rpc");
        AppendConfig("rpc-listen-all", "true");
        AppendConfig("rpc-allow-origin-all","true");
        AppendConfig("check-certificate", "false");
        // * port
        AppendConfig("rpc-listen-port", ListenPort.ToString());
        // * token
        AppendConfig("rpc-secret", Token);
        // * max-concurrent-downloads
        AppendConfig("max-concurrent-downloads", MaxConcurrentDownloads.ToString());
        // * max-connection-per-server
        AppendConfig("max-connection-per-server", MaxConnectionPerServer.ToString());
        // * split
        AppendConfig("split", Split.ToString());
        // * min-split-size
        AppendConfig("min-split-size", MinSplitSize.ToString() + "M");
        // * max-overall-download-limit
        AppendConfig("max-overall-download-limit", MaxOverallDownloadLimit.ToString());
        // * max-download-limit
        AppendConfig("max-download-limit", MaxDownloadLimit.ToString());
        // * max-overall-upload-limit
        AppendConfig("max-overall-upload-limit", MaxOverallUploadLimit.ToString());
        // * max-upload-limit
        AppendConfig("max-upload-limit", MaxUploadLimit.ToString());
        // * continue
        AppendConfig("continue", ContinueDownload.ToString().ToLower());
        // * file allocate way
        AppendConfig("file-allocation", FileAllocation.ToString("G").ToLower());
        // * log 
        if (!string.IsNullOrEmpty(LogFilePath)) {
            AppendConfig("log", $"\"{this.LogFilePath}\"");
            AppendConfig("log-level", LogLevel.ToString("G").ToLower());
        }
        return configBuilder.ToString();
    }
}