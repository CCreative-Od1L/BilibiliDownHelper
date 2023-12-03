using Core.Utils;

namespace Core.DirectoryFunc {
    public sealed class DirectoryManager {
        public FileDirectory fileDirectory;
        readonly string relativeFileDirectoryJsonPath = @"dir.json";
        public DirectoryManager() {
            fileDirectory = new();
            string dirJsonString = FileUtils.ReadTextFile(Path.Combine(fileDirectory.Root, relativeFileDirectoryJsonPath));
            if (string.IsNullOrEmpty(dirJsonString)) {
                TryToInit();
            } else {
                UpdateFileDirectory(JsonUtils.ParseJsonString<FileDirectory>(dirJsonString));
            }
            JsonUtils.WriteJsonInto(fileDirectory, Path.Combine(fileDirectory.Root, relativeFileDirectoryJsonPath));
        }
        void TryToInit() {
            fileDirectory.TryToResetDefault();
        }

        void UpdateFileDirectory(FileDirectory? newFileDirectory) {
            if (newFileDirectory == null) { 
                fileDirectory.TryToResetDefault();
                return; 
            }
            fileDirectory.UpdateData(newFileDirectory);
        }
        public void ResetToDefault(string name) {
            fileDirectory.ResetDefault(name);
        }
        public string GetLogDirectory() {
            TryToInit();
            return fileDirectory.Log!;
        }
        public string GetCookieDirectory() {
            TryToInit();
            return fileDirectory.Cookie!;
        }
    }
}