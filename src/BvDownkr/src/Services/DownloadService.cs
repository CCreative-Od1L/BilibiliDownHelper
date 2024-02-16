using BvDownkr.src.Utils;
using Core;
using Core.Aria2cNet.Client.Entity;
using Core.Aria2cNet.Client;
using Core.Aria2cNet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Aria2cNet.AriaManager;
using Core.Aria2cNet;
using Core.Utils;
using System.IO;
using Core.FileFunc;
using BvDownkr.src.Entries;

namespace BvDownkr.src.Services
{
    public class DownloadService {
        public static DownloadService INSTANCE { get; private set; } = new();
        private bool IsServerStart = false;
        private readonly string DfRecord = "WB4wQYP2.dat";
        public async Task OpenServerAsync() {
            if (IsServerStart) return;
            else {
                var option = new Aria2Config();
                option.AddBilibiliReferer();

                var openServerResult = await ServerSingleton.Instance.AsyncStartServer(
                    option,
                    CoreManager.logger.Info
                );
                IsServerStart = openServerResult;
            }
        }
        public async Task CloseServer() {
            if (!IsServerStart) { return; }
            var closeResult = await ServerSingleton.Instance.CloseServerAsync();
            if (closeResult) {
                IsServerStart = false;
            }
        }
        public static void AddTellStatusAction(TellStatusHandler tellStatusAction) {
            CoreManager.ariaMgr.TellStatus += tellStatusAction;
        }
        public static void RemoveTellStatusAction(TellStatusHandler tellStatusAction) {
            CoreManager.ariaMgr.TellStatus -= tellStatusAction;
        }
        public static void AddDownloadFinishAction(DownloadFinishHandler downloadFinishAction) {
            CoreManager.ariaMgr.DownloadFinish += downloadFinishAction;
        }
        public static void RemoveDownloadFinishAction(DownloadFinishHandler downloadFinishAction) {
            CoreManager.ariaMgr.DownloadFinish -= downloadFinishAction;
        }
        public static void AddGlobalStatusAction(GetGlobalStatusHandler getGlobalStatusAction) {
            CoreManager.ariaMgr.GlobalStatus += getGlobalStatusAction;
        }
        public static void RemoveGlobalStatusAction(GetGlobalStatusHandler getGlobalStatusAction) {
            CoreManager.ariaMgr.GlobalStatus -= getGlobalStatusAction;
        }
        /// <summary>
        /// * 下载url的文件
        /// </summary>
        /// <param name="url">目标URL</param>
        /// <param name="fileDir">下载目录</param>
        /// <param name="fileName">文件名称（包含后缀）</param>
        /// <returns>下载文件的gid（需管理）</returns>
        public async Task<string> DownloadSomethingAsync(
            List<string> url,
            string fileDir, 
            string? fileName = null
        ) {
            if (IsServerStart == false) { return string.Empty; }

            if (string.IsNullOrEmpty(fileName)) {
                fileName = RegexMethod.GetUrlFileName(url.FirstOrDefault(""));
            }
            FileUtils.CheckAndCreateDirectory(fileDir);
            var downTask = await ClientSingleton.Instance.AddUriAsync(
                url,
                new AriaSendOption() {
                    Dir = fileDir,
                    Out = fileName,
                }
            );
            if (downTask != null) {
                string gid = downTask.Result;
                AriaManager.INSTANCE.GetDownloadStatus(gid);
                return gid;
            }
            return string.Empty;
        }
        /// <summary>
        /// * 记录DfRecord
        /// * 暂定由 DownloadTaskVM定时调用
        /// </summary>
        /// <param name="records"></param>
        public void WriteDfRecord(Dictionary<string, DfRecordEntry> records) {
            List<DataForm> buf = [];
            foreach (var record in records) {
                buf.Add(new(
                    name: record.Key,
                    content: JsonUtils.SerializeJsonObj(record.Value),
                    enableCrypt: false));
            }
            Task writeTask = new(async obj => {
                await FileUtils.AsyncUpdateFile(Path.Combine(CoreManager.directoryMgr.fileDirectory.Download, DfRecord), buf);
            }, null, TaskCreationOptions.PreferFairness);
            writeTask.Start();
        }
        public Dictionary<string, DfRecordEntry> ReadDfRecord() {
            Dictionary<string, DfRecordEntry> buf = [];
            var result = FileUtils.ReadFile(Path.Combine(
                    CoreManager.directoryMgr.fileDirectory.Download,
                    DfRecord
                ));
            foreach(var record in result) {
                var data = record.Value;
                var dataContent = JsonUtils.ParseJsonString<DfRecordEntry>(data.Content);
                if (dataContent != null) {
                    buf.Add(data.Name, dataContent);
                }
            }
            return buf;
        }
    }
}
