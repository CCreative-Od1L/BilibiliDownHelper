namespace Core.Cookie {
    internal sealed class CookieManager {
        readonly string CookieDirectory;
        string _cookieFilePath;
        readonly AutoResetEvent Pause = new(true);
        AutoResetEvent? CookieMgrStopLock;
        FileSystemWatcher? cookieWatcher;
        readonly CancellationTokenSource cookieWatcherCTS;
        public string CookieFilePath {
            get => _cookieFilePath; 
            private set {
                _cookieFilePath = value;
            }
        }
        Dictionary<string, CookieData>? cookies;
        public CookieManager() {
            cookies = [];
            cookieWatcherCTS = new();

            CookieDirectory = CoreManager.directoryMgr.GetCookieDirectory();
            if (!System.IO.Directory.Exists(CookieDirectory)) {
                System.IO.Directory.CreateDirectory(CookieDirectory);
            } 
            _cookieFilePath = Path.Combine(CoreManager.directoryMgr.GetCookieDirectory(), "cookies");
            CreateEmptyCookieFile();
            
            // ! the latest call.
            StartTask();
        }

        void OnCookieFileDeleted(object sender, FileSystemEventArgs e) {
            CoreManager.logger.Info(nameof(OnCookieFileDeleted), "Cookies文件被删除，尝试重新生成空文件");
            CreateEmptyCookieFile();
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
                cookieWatcher = new FileSystemWatcher(_cookieFilePath);
                cookieWatcher.Deleted += OnCookieFileDeleted;
                cookieWatcher.Changed += OnCookieFileDeleted;

                while(true) {
                    Pause.WaitOne(10000, true);
                    if(cookieWatcherCancelToken.IsCancellationRequested) {
                        cookieWatcher.Deleted -= OnCookieFileDeleted;
                        cookieWatcher.Changed -= OnCookieFileDeleted;
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
                        CoreManager.logger.Info(nameof(CookieData), "初始化cookie失败, cookieData.CookieName为空。");
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
                CoreManager.logger.Info(nameof(TryToGetCookiesData), "cookieFilePath下不存在cookies文件");
            }
            return cookies;
        }
    }
}