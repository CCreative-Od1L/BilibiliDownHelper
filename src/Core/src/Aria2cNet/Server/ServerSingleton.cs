using System.Diagnostics;
using System.Text;
using Core.Aria2cNet.Client;
using Core.Utils;

namespace Core.Aria2cNet.Server;
public class ServerSingleton {
    public static ServerSingleton Instance { get; } = new();
    private int ListenPort;             // * 服务器端口
    private Process? ServerProcess;     // * 服务器线程
    /// <summary>
    /// * 异步开启 Aria2c服务器
    /// </summary>
    /// <param name="config"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task<bool> AsyncStartServer(Aria2Config config, Action<string> action) {
        ListenPort = config.ListenPort;
        string AriaDirectory = Path.Combine(CoreManager.directoryMgr.fileDirectory.ThirdParty, @"aria\");
        FileUtils.CheckAndCreateDirectory(AriaDirectory);

        string sessionFilePath = Path.Combine(AriaDirectory, "aria.session");
        int sessionSaveTimeInterval = 30;
        string logFile = Path.Combine(AriaDirectory, "aria.log");
        FileUtils.TryToCreateFile(sessionFilePath);
        FileUtils.TryToCreateFile(logFile);
        long MaxLogFileLen = 10 * 1024 * 1024L;
        config.LogFilePath = logFile;
        config.LogLevel = Aria2ConfigLogLevel.DEBUG;

        await Task.Run(() => {
            using (var logFileStream = File.OpenWrite(logFile)) {
                if (logFileStream.Length > MaxLogFileLen) {
                    logFileStream.SetLength(0);
                }
            }
            StringBuilder configBuilder = new(config.ToString());
            // session config
            configBuilder.Append($"--input-file=\"{sessionFilePath}\" --save-session=\"{sessionFilePath}\" ");
            configBuilder.Append($"--save-session-interval={sessionSaveTimeInterval} ");

            ExecuteProcess(
                "aria2c.exe", 
                configBuilder.ToString(),
                null,
                (s, e) => {
                    if (string.IsNullOrWhiteSpace(e.Data)) { return; }
                    CoreManager.logger.Debug("AriaServer", e.Data);

                    action.Invoke(e.Data);
                }
            );
        });
        return true;
    }
    /// <summary>
    /// * 服务器端口返回
    /// </summary>
    /// <returns></returns>
    public string ShowServerPort() {
        return ListenPort.ToString();
    }
    /// <summary>
    /// * 异步关闭服务器
    /// </summary>
    /// <returns></returns>
    public async Task<bool> CloseServerAsync() {
        if (ServerProcess == null) { return true; }

        await ClientSingleton.Instance.ShutdownAsync();
        return await ProcessUtils.AsyncKillProcess(process: ServerProcess);
    }
    /// <summary>
    /// * 生成进程配置
    /// </summary>
    /// <param name="exeName"></param>
    /// <param name="arg"></param>
    /// <param name="workDirectory"></param>
    /// <returns></returns>
    private static ProcessStartInfo BuildProcessStartInfo(
        string exeName,
        string arg,
        string? workDirectory
    ) {
        return new ProcessStartInfo() {
            FileName = exeName,
            Arguments = arg,
            WorkingDirectory = workDirectory??"",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
        };
    }
    /// <summary>
    /// * 开启进程
    /// </summary>
    /// <param name="exeName"></param>
    /// <param name="arg"></param>
    /// <param name="workDirectory"></param>
    /// <param name="output"></param>
    private void ExecuteProcess(
        string exeName,
        string arg,
        string? workDirectory,
        DataReceivedEventHandler output
    ) {
        ServerProcess = new() {
            StartInfo = BuildProcessStartInfo(exeName, arg, workDirectory),
        };
        ServerProcess.OutputDataReceived += output;
        ServerProcess.ErrorDataReceived += output;

        ProcessUtils.StartProcess(process: ServerProcess);
    }

}