using Core.Utils;

namespace Core.Directory {
    internal static class DirectoryManager {
        static public FileDirectory fileDirectory;
        static readonly string relativeFileDirectoryJsonPath = @"dir.json";
        static DirectoryManager() {
            fileDirectory = new();
            string dirJsonString = FileUtils.ReadFile(Path.Combine(fileDirectory.Root, relativeFileDirectoryJsonPath));
            if (string.IsNullOrEmpty(dirJsonString)) {
                TryToInit();
            } else {
                UpdateFileDirectory(JsonUtils.ParseJsonString<FileDirectory>(dirJsonString));
            }
            JsonUtils.WriteJsonInto(fileDirectory, Path.Combine(fileDirectory.Root, relativeFileDirectoryJsonPath));
        }
        static void TryToInit() {
            fileDirectory.TryToResetDefault();
        }

        static void UpdateFileDirectory(FileDirectory? newFileDirectory) {
            if (newFileDirectory == null) { 
                fileDirectory.TryToResetDefault();
                return; 
            }
            fileDirectory.UpdateData(newFileDirectory);
        }
        static public void ResetToDefault(string name) {
            fileDirectory.ResetDefault(name);
        }
        static public string GetLogDirectory() {
            TryToInit();
            return fileDirectory.Log!;
        }
        static public string GetCookieDirectory() {
            TryToInit();
            return fileDirectory.Cookie!;
        }
    }
}