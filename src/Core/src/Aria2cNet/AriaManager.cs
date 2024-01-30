using Core.Aria2cNet.Client;

namespace Core.Aria2cNet;
public class AriaManager {
    public static AriaManager Instance { get; } = new();
    // gid对应项目的状态
    public delegate void TellStatusHandler(string gid, long totalLength, long completedLength, long speed);
    public event TellStatusHandler? TellStatus;
    protected virtual void OnTellStatus(string gid, long totalLength, long completedLength, long speed) {
        TellStatus?.Invoke(gid, totalLength, completedLength, speed);
    }

    // 下载结果回调
    public delegate void DownloadFinishHandler(string gid, bool isSuccess, string downloadPath, string? msg = null);
    public event DownloadFinishHandler? DownloadFinish;
    protected virtual void OnDownloadFinish(string gid, bool isSuccess, string downloadPath, string? msg = null) {
        DownloadFinish?.Invoke(gid, isSuccess, downloadPath, msg);
    }

    // 全局下载状态
    public delegate void GetGlobalStatusHandler(long speed);
    public event GetGlobalStatusHandler? GlobalStatus;
    protected virtual void OnGlobalStatus(long speed) {
        GlobalStatus?.Invoke(speed);
    }
    public enum DownloadResult {
        SUCCESS = 1,
        FAILED,
        ABORT,
    }
    /// <summary>
    /// * 异步持续获取（更新）下载状态
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task AsyncGetDownloadStatus(string gid, AutoResetEvent stop, Action? action = null) {
        AutoResetEvent Pause = new(false);
        string filePath = string.Empty;
        while (true) {
            var status = await ClientSingleton.Instance.TellStatus(gid);
            if (status == null) { continue; }

            // * 返回结果为空且有错误信息
            if (status.Result == null && status.Error != null) {
                if (status.Error.Message.Contains("is not found")) {
                    OnDownloadFinish(
                        isSuccess: false,
                        downloadPath: string.Empty,
                        gid: gid,
                        msg: status.Error.Message
                    );
                    stop.Set();
                    return;
                }
            }

            if (status.Result != null) {
                if (status.Result.Files.Count >= 1) {
                    filePath = status.Result.Files[0].Path;
                }

                long totalLength = long.Parse(status.Result.TotalLength);
                long completedLength = long.Parse(status.Result.CompletedLength);
                long speed = long.Parse(status.Result.DownloadSpeed);
                // * 进度通知
                OnTellStatus(gid, totalLength, completedLength, speed);
                
                action?.Invoke();
                if (status.Result.Status == "complete") { break; }
                // * 错误码不为 0
                if (!string.IsNullOrEmpty(status.Result.ErrorCode) && !status.Result.ErrorCode.Equals("0")) {
                    CoreManager.logger.Error("AriaManager", status.Result.ErrorMessage);
                    var removeResult = await ClientSingleton.Instance.RemoveDownloadResultAsync(gid);
                    if (removeResult != null) {
                        CoreManager.logger.Debug("AriaManager", removeResult.Result);
                    }

                    OnDownloadFinish(
                        gid: gid,
                        isSuccess: false,
                        downloadPath: string.Empty,
                        status.Result.ErrorMessage
                    );
                    stop.Set();
                    return;
                }
            }
            Pause.WaitOne(500, true);
        }
        OnDownloadFinish(
            gid: gid,
            isSuccess: true,
            downloadPath: filePath,
            string.Empty
        );
        stop.Set();
    }
    /// <summary>
    /// * 获取全局下载速度
    /// </summary>
    /// <returns></returns>
    public async void GetGlobalStatus() {
        AutoResetEvent Pause = new(false);
        while (true) {
            var globalStatus = await ClientSingleton.Instance.GetGlobalStatusAsync();
            if (globalStatus == null || globalStatus.Result == null) { continue; }

            long globalSpeed = long.Parse(globalStatus.Result.DownloadSpeed);
            OnGlobalStatus(globalSpeed);
            Pause.WaitOne(500, true);
        }
    }
}