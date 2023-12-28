namespace Core.Aria2cNet;
public class AriaManager {
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
}