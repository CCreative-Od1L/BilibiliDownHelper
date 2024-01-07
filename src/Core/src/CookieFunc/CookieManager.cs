using System.Net;
using Core.Utils;

namespace Core.CookieFunc {
    public sealed class CookieManager {
        public static CookieManager INSTANCE { get; } = new();
        readonly string CookieDirectory;
        string _cookieFilePath;
        readonly AutoResetEvent Pause = new(true);
        AutoResetEvent? CookieMgrStopLock;
        FileSystemWatcher? cookieWatcher;
        readonly CancellationTokenSource cookieWatcherCTS;
        public string? RefreshToken { get; private set; }
        DateTime RefreshTokenUpdateTime { get; set; }
        public string CookieFilePath {
            get => _cookieFilePath; 
            private set {
                _cookieFilePath = value;
            }
        }
        CookieCollection cookies;
        private CookieManager() {
            cookies = [];
            cookieWatcherCTS = new();

            CookieDirectory = CoreManager.directoryMgr.GetCookieDirectory();
            if (!Directory.Exists(CookieDirectory)) {
                Directory.CreateDirectory(CookieDirectory);
            }

            _cookieFilePath = Path.Combine(CoreManager.directoryMgr.GetCookieDirectory(), "VxkUMc3Q.dat");
            
            if (File.Exists(_cookieFilePath) && File.ReadAllBytes(_cookieFilePath).Length == 0) {
                File.Delete(_cookieFilePath);
            }
            // RefreshToken 读取 / 更新

            // ! the latest call.
            StartTask();
        }
        void OnCookieFileDeleted(object sender, FileSystemEventArgs e) {
            CoreManager.logger.Info(nameof(OnCookieFileDeleted), "Cookies文件被删除，尝试重新生成空文件");
            if (CheckCookieFileExist()) {
                UpdateCookiesData();
            }
        }
        void OnCookieFileChanged(object sender, FileSystemEventArgs e) {
            CoreManager.logger.Info(nameof(OnCookieFileChanged), "Cookies文件有更新，更新Cookie数据");
            if(CheckCookieFileExist()) {
                UpdateCookiesData();
            }
        }
        void StartTask() {
            var cookieWatcherCancelToken = cookieWatcherCTS.Token;
            CookieMgrStopLock = new(false);

            Task watchCookieFileChange = new(obj => {
                cookieWatcher = new FileSystemWatcher(CookieDirectory);
                cookieWatcher.Deleted += OnCookieFileDeleted;
                cookieWatcher.Changed += OnCookieFileChanged;

                while(true) {
                    Pause.WaitOne(10000, true);
                    if(cookieWatcherCancelToken.IsCancellationRequested) {
                        cookieWatcher.Deleted -= OnCookieFileDeleted;
                        cookieWatcher.Changed -= OnCookieFileChanged;
                        CookieMgrStopLock.Set();
                        break;
                    }
                }
            }, null, TaskCreationOptions.LongRunning);
            watchCookieFileChange.Start();
        }
        void StopTask() {
            if (CookieMgrStopLock == null) { return; }
            Task cookieMgrStopTask = new(() => {
                cookieWatcherCTS.Cancel();
                CookieMgrStopLock.WaitOne();
                cookieWatcherCTS.Dispose();
            });
            cookieMgrStopTask.Start();
        }
        public bool CheckCookieFileExist() {
            return File.Exists(_cookieFilePath) || File.ReadAllBytes(_cookieFilePath).Length <= 0;
        }
        void UpdateCookiesData() {
            // TODO 修正
            string cookieFileString = FileUtils.ReadBytesThenDecryptToText(_cookieFilePath);
            if (string.IsNullOrEmpty(cookieFileString)) { return; }

            using var sr = new StringReader(cookieFileString);
            string? line = string.Empty;
            while((line = sr.ReadLine()) != null) {
                CookieData cookieData = new(line);
                if (cookieData.Cookie == null) {
                    CoreManager.logger.Info(nameof(CookieData), "初始化cookie失败, cookieData.CookieName为空。");
                    return;
                }
                if (cookies.Contains(cookieData.Cookie)) {
                    cookies.Remove(cookieData.Cookie);
                }
                cookies.Add(cookieData.Cookie);
            }
        }
        public CookieCollection TryToGetCookies() {
            if (CheckCookieFileExist()) {
                UpdateCookiesData();
            } else {
                cookies = [];
                CoreManager.logger.Info(nameof(TryToGetCookies), "cookieFilePath下不存在有效的cookies文件");
            }
            return cookies;
        }
        public void TryToUpdateCookies(Action? callback = null) {
            if (CheckCookieFileExist()) {
                UpdateCookiesData();
                callback?.Invoke();
            }
        }
        public void UpdateRefreshTokenData(string token, string updateTimestampStr) {
            RefreshToken = token;
            RefreshTokenUpdateTime = DateTimeUtils.TimestampToDateTime(updateTimestampStr);
            // TODO 文件保存
            
        }
        // TODO 从文件中读取到最新的 refresh_token 以及 update_time
        void GetRefreshTokenAndUpdateTime() {

        }
        // TODO
        void CheckCookieRefresh() {

        }
    }
}