using Core.Directory;
using Core.Logger;
using Core.PleInterface;

namespace Core.Cookie {
    sealed class CookieManager : ManagerBase {
        readonly string CookieDirectory;
        string _cookieFilePath;
        AutoResetEvent Pause => new(true);
        FileSystemWatcher? cookieWatcher;
        CancellationTokenSource? cookieWatcherCTS;
        public string CookieFilePath {
            get => _cookieFilePath; 
            private set {
                _cookieFilePath = value;
            }
        }
        static Dictionary<string, CookieData>? cookies;
        public CookieManager() {
            cookies = [];
            CookieDirectory = DirectoryManager.GetCookieDirectory();
            if (!System.IO.Directory.Exists(CookieDirectory)) {
                System.IO.Directory.CreateDirectory(CookieDirectory);
            } 
            _cookieFilePath = Path.Combine(DirectoryManager.GetCookieDirectory(), "cookies");
            CreateEmptyCookieFile();
            
            // ! the latest call.
            StartTask();
        }
        void OnCookieFileDeleted(object sender, FileSystemEventArgs e) {
            LogManager.Info(nameof(OnCookieFileDeleted), "Cookies文件被删除，尝试重新生成空文件");
            CreateEmptyCookieFile();
            if (CheckCookieFileExist()) {
                UpdateCookiesData();
            }
        }
        void OnCookieFileChanged(object sender, FileSystemEventArgs e) {
            LogManager.Info(nameof(OnCookieFileChanged), "Cookies文件有更新，更新Cookie数据");
            if(CheckCookieFileExist()) {
                UpdateCookiesData();
            }
        }
        // TODO 考虑接口
        void StartTask() {
            cookieWatcherCTS = new();
            var cookieWatcherCancelToken = cookieWatcherCTS.Token;
            
            Task watchCookieFileChange = new(obj => {
                cookieWatcher = new FileSystemWatcher(_cookieFilePath);
                cookieWatcher.Deleted += OnCookieFileDeleted;
                cookieWatcher.Changed += OnCookieFileDeleted;

                while(true) {
                    Pause.WaitOne(10000, true);
                    if(cookieWatcherCancelToken.IsCancellationRequested) {
                        cookieWatcher.Deleted -= OnCookieFileDeleted;
                        cookieWatcher.Changed -= OnCookieFileDeleted;
                        break;
                    }
                }
            }, null, TaskCreationOptions.LongRunning);
            watchCookieFileChange.Start();
        }
        // TODO 考虑接口
        void StopTask() {
            cookieWatcherCTS?.Cancel();
            cookieWatcherCTS?.Dispose();
        }

        public void CreateEmptyCookieFile() {
            if (!File.Exists(_cookieFilePath)) {
                File.Create(_cookieFilePath);
            }
        }
        public bool CheckCookieFileExist() {
            return File.Exists(_cookieFilePath);
        }
        void UpdateCookiesData() {
            {
                using var sr = new StreamReader(_cookieFilePath);
                string? line = string.Empty;
                while((line = sr.ReadLine()) != null) {
                    CookieData cookieData = new(line);
                    if (string.IsNullOrEmpty(cookieData.CookieName)) {
                        LogManager.Info(nameof(CookieData), "初始化cookie失败, cookieData.CookieName为空。");
                        return;
                    }
                    if (!cookies!.TryAdd(cookieData.CookieName, cookieData)) {
                        cookies[cookieData.CookieName] = cookieData;
                    }
                }
            }
        }
        public Dictionary<string, CookieData>? TryToGetCookiesData() {
            if (CheckCookieFileExist()) {
                UpdateCookiesData();
            } else {
                cookies = null;
                LogManager.Info(nameof(TryToGetCookiesData), "cookieFilePath下不存在cookies文件");
            }
            return cookies;
        }
    }
}