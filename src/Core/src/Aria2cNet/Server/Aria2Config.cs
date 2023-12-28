using System.Text;
using System.Text.RegularExpressions;

namespace Core.Aria2cNet.Server;
public enum Aria2ConfigLogLevel {
    NOT_SET = 0,
    DEBUG = 1,
    INFO,
    NOTICE,
    WARN,
    ERROR,
}
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
    public int MinSplitSize { 
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
    }
    public void SetLogConfig(string filePath, Aria2ConfigLogLevel level = Aria2ConfigLogLevel.INFO) {
        if (string.IsNullOrEmpty(filePath)) { return; }
        LogFilePath = filePath;
        LogLevel = level;
    }
    public override string ToString()
    {
        StringBuilder configBuilder = new();
        // header
        if (Headers != null) {
            for(int i = 0; i < Headers.Count; ++i) {
                configBuilder.Append($"--header=\"{Headers[i]}\"");
            }
        }
        // default config
        configBuilder.Append("--enable-rpc --rpc-listen-all=true --rpc-allow-origin-all=true --check-certificate=false ");
        // port
        configBuilder.Append($"--rpc-listen-port={ListenPort} ");
        // token
        configBuilder.Append($"--rpc-secret={Token} ");
        // max-concurrent-downloads
        configBuilder.Append($"--max-concurrent-downloads={this.MaxConcurrentDownloads} ");
        // max-connection-per-server
        configBuilder.Append($"--max-connection-per-server={this.MaxConnectionPerServer} ");
        // split
        configBuilder.Append($"--split={this.Split} ");
        // min-split-size
        configBuilder.Append($"--min-split-size={this.MinSplitSize}M ");
        // max-overall-download-limit
        configBuilder.Append($"--max-overall-download-limit={this.MaxOverallDownloadLimit} ");
        // max-download-limit
        configBuilder.Append($"--max-download-limit={this.MaxDownloadLimit} ");
        // max-overall-upload-limit
        configBuilder.Append($"max-overall-upload-limit={this.MaxOverallUploadLimit} ");
        // max-upload-limit
        configBuilder.Append($"--max-upload-limit={this.MaxUploadLimit} ");
        // continue
        configBuilder.Append($"--continue={this.ContinueDownload.ToString().ToLower()} ");
        // file allocate way
        configBuilder.Append($"--file-allocation={this.FileAllocation.ToString("G").ToLower()} ");
        // log 
        if (!string.IsNullOrEmpty(LogFilePath)) {
            configBuilder.Append($"--log=\"{this.LogFilePath}\" --log-level={this.LogLevel.ToString("G").ToLower()} ");
        }
        return configBuilder.ToString();
    }
}