namespace Core.Storage {
    public class FileDirectory {
        public string Root {get; private set;}
        public string? Log {get; set;}
        public FileDirectory() {
            Root = AppDomain.CurrentDomain.BaseDirectory + @"AppData\";
        }
        public void UpdateData(FileDirectory fileDirectory) {
            Log = fileDirectory.Log??Log;
        }
        public void TryToResetDefault(){
            if (string.IsNullOrEmpty(Root)) { Root = AppDomain.CurrentDomain.BaseDirectory + @"AppData\"; }
            if (string.IsNullOrEmpty(Log)) { Log = Root + @"Logs\"; }
        }
        public void TryToResetDefault(string name) {
            switch(name.ToLower()) {
                case "log":
                    Log = Root + @"Logs\";
                    break;
                default:
                    break;
            }
        }
    }
}