using System.Net;
using Core.CookieFunc.Model;
using Core.FileFunc;
using Core.Utils;

namespace Core.CookieFunc {
    public sealed class CookieManager {
        public static CookieManager INSTANCE { get; } = new();
        /// <summary>
        /// * Cookie相关文件的文件夹路径
        /// </summary>
        readonly string CookieDirectory;
        /// <summary>
        /// * Cookie文件的文件路径
        /// </summary>
        string _cookieFilePath;
        readonly AutoResetEvent Pause = new(true);
        AutoResetEvent? CookieMgrStopLock;
        /// <summary>
        /// * Cookie文件夹观察者-响应文件夹内变化
        /// </summary>
        FileSystemWatcher? cookieWatcher;
        readonly CancellationTokenSource cookieWatcherCTS;

        public Dictionary<string, DataForm> cookieFileData;

        /// <summary>
        /// * Refresh Token 相关数据
        /// </summary>
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
            cookieFileData = [];

            CookieDirectory = CoreManager.directoryMgr.GetCookieDirectory();
            _cookieFilePath = Path.Combine(CoreManager.directoryMgr.GetCookieDirectory(), "VxkUMc3Q.dat");
            FileUtils.CheckAndCreateFileDirectory(_cookieFilePath);
            
            InitFileData();
            // * CookieFile 读取 / 更新
            if (!File.Exists(_cookieFilePath)) {
                OverwriteDataToFile();
            } else {
                GetFileDataFromFile();
            }

            // ! the latest call.
            StartTask();
        }
        /// <summary>
        /// * Cookie文件夹内发生文件删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCookieFileDeleted(object sender, FileSystemEventArgs e) {
            CoreManager.logger.Info(nameof(OnCookieFileDeleted), "Cookies文件被删除，尝试重新生成空文件");
            if (CheckCookieFileExist()) {
                UpdateCookiesData();
            }
        }
        /// <summary>
        /// * Cookie文件夹内发生文件修改操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCookieFileChanged(object sender, FileSystemEventArgs e) {
            CoreManager.logger.Info(nameof(OnCookieFileChanged), "Cookies文件有更新，更新Cookie数据");
            if(CheckCookieFileExist()) {
                UpdateCookiesData();
            }
        }
        /// <summary>
        /// * 开始任务-各种初始化
        /// </summary>
        void StartTask() {
            var cookieWatcherCancelToken = cookieWatcherCTS.Token;
            CookieMgrStopLock = new(false);

            Task watchCookieFileChange = new(obj => {
                cookieWatcher = new FileSystemWatcher(CookieDirectory);
                cookieWatcher.Deleted += OnCookieFileDeleted;
                cookieWatcher.Changed += OnCookieFileChanged;

                while(true) {
                    Pause.WaitOne(10000, true);     // * 10秒检测一次是否退出
                    if(cookieWatcherCancelToken.IsCancellationRequested) {
                        cookieWatcher.Deleted -= OnCookieFileDeleted;
                        cookieWatcher.Changed -= OnCookieFileChanged;
                        CookieMgrStopLock.Set();
                        break;
                    }
                    // * 将当前数据push到文件中
                    UpdateDataToFile();
                }
            }, null, TaskCreationOptions.LongRunning);
            watchCookieFileChange.Start();
        }
        /// <summary>
        /// * 停止任务
        /// </summary>
        void StopTask() {
            if (CookieMgrStopLock == null) { return; }
            Task cookieMgrStopTask = new(() => {
                cookieWatcherCTS.Cancel();
                CookieMgrStopLock.WaitOne();
                cookieWatcherCTS.Dispose();
            });
            cookieMgrStopTask.Start();
        }
        /// <summary>
        /// * 检测Cookie文件是否存在
        /// </summary>
        /// <returns></returns>
        public bool CheckCookieFileExist() {
            return File.Exists(_cookieFilePath) || File.ReadAllBytes(_cookieFilePath).Length <= 0;
        }
        /// <summary>
        /// * 更新Cookie数据
        /// </summary>
        void UpdateCookiesData() {
            string? cookieFileString = GetCookiesStr();
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
        /// <summary>
        /// * 尝试获取CookieCollection(Cookie集合)
        /// </summary>
        /// <returns></returns>
        public CookieCollection TryToGetCookies() {
            if (CheckCookieFileExist()) {
                UpdateCookiesData();
            } else {
                cookies = [];
                CoreManager.logger.Info(nameof(TryToGetCookies), "cookieFilePath下不存在有效的cookies文件");
            }
            return cookies;
        }
        /// <summary>
        /// * IO操作-读数据
        /// </summary>
        public void GetFileDataFromFile() {
            cookieFileData = FileUtils.ReadFile(_cookieFilePath);
        }
        /// <summary>
        /// * IO操作-重写数据
        /// </summary>
        public void OverwriteDataToFile() {
            List<DataForm> buf = [];
            foreach(var item in cookieFileData) {
                buf.Add(item.Value);
            }
            _ = FileUtils.AsyncWriteFile(_cookieFilePath, buf);
        }
        /// <summary>
        /// * IO操作-更新数据
        /// </summary>
        public void UpdateDataToFile() {
            List<DataForm> buf = [];
            foreach(var item in cookieFileData) {
                buf.Add(item.Value);
            }
            _ = FileUtils.AsyncUpdateFile(_cookieFilePath, buf);
        }
        /// <summary>
        /// * 初始化数据格式（除具体内容）
        /// </summary>
        private void InitFileData() {
            // * Cookie
            InitSingleData(CookieFileDataNames.Cookie, true);
            // * RefreshToken
            InitSingleData(CookieFileDataNames.RefreshToken, true);
            // * RefreshTokenUpdateTime
            InitSingleData(CookieFileDataNames.RefreshTokenUpdateTime, false);
        }
        private void InitSingleData(string name, bool enableCrypt) {
            cookieFileData.TryAdd(name ,new());
            cookieFileData[name].Name = name;
            cookieFileData[name].EnableCrypt = enableCrypt;
        }
        /// <summary>
        /// * 尝试更新CookieCollection
        /// </summary>
        /// <param name="callback"></param>
        public void TryToUpdateCookies(Action? callback = null) {
            if (CheckCookieFileExist()) {
                UpdateCookiesData();
                callback?.Invoke();
            }
        }
        public void UpdateCookiesToFile(string cookieStr) {
            cookieFileData[CookieFileDataNames.Cookie].Content = cookieStr;
        }
        public string? GetCookiesStr() {
            cookieFileData.TryGetValue(CookieFileDataNames.Cookie, out DataForm? res);
            return res?.Content;
        }
        /// <summary>
        /// * 更新 RefreshToken数据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="updateTimestampStr"></param>
        public void UpdateRefreshTokenData(string token, string updateTimestampStr) {
            cookieFileData[CookieFileDataNames.RefreshToken].Content = token;
            cookieFileData[CookieFileDataNames.RefreshTokenUpdateTime].Content = updateTimestampStr;

            RefreshTokenUpdateTime = DateTimeUtils.TimestampToDateTime(updateTimestampStr);
        }
        /// <summary>
        /// * 读取到最新的 refresh_token 以及 update_time
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="refreshDateTime"></param>
        /// <returns></returns>
        void GetRefreshTokenAndUpdateTime(out string? refreshToken, out DateTime refreshDateTime) {
            cookieFileData.TryGetValue(CookieFileDataNames.RefreshToken, out var refreshTokenData);
            refreshToken = refreshTokenData?.Content;

            cookieFileData.TryGetValue(CookieFileDataNames.RefreshTokenUpdateTime, out var refreshDateTimeData);
            if ((refreshDateTimeData != null) && string.IsNullOrEmpty(refreshDateTimeData.Content)) {
                refreshDateTime = DateTimeUtils.TimestampToDateTime(refreshDateTimeData.Content);
            } else {
                refreshDateTime = DateTimeUtils.GetDefaultDateTime(); 
            }
        }
        public async Task<Tuple<bool, long>> CheckCookieRefresh() {
            cookieFileData.TryGetValue(CookieFileDataNames.Cookie, out var cookieData);
            cookieFileData.TryGetValue(CookieFileDataNames.RefreshToken, out var oldRefreshToken);

            if (string.IsNullOrEmpty(cookieData!.Content) || string.IsNullOrEmpty(oldRefreshToken!.Content)) {
                return new(false, 0);
            }
            
            CheckCookieResponse? checkCookieResponse = null;
            string url = "https://passport.bilibili.com/x/passport-login/web/cookie/info";
            var result = await Web.WebClient.Request(url, "get");
            if (result.Item1 != false) {
                checkCookieResponse = JsonUtils.ParseJsonString<CheckCookieResponse>(result.Item2);
            }

            if (checkCookieResponse == null) { return new(false, 0); }
            else { return new(checkCookieResponse.CheckIfRefresh(), checkCookieResponse.GetTimestamp()); }
        }
    }
}