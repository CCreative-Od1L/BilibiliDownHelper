using Core.Utils;

namespace Core.Storage {
    internal static class StorageManager {
        static public FileDirectory fileDirectory;
        static readonly string relativeFileDirectoryJsonPath = @"dir.json";
        static StorageManager() {
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
    }
}