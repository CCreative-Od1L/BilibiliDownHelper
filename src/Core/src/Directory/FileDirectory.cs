namespace Core.Directory {
    public class FileDirectory {
        public string Root {get; private set;}
        public string? Log {get; set;}
        public string? Cookie {get; set;}
        public FileDirectory() {
            Root = AppDomain.CurrentDomain.BaseDirectory + @"AppData\";
        }
        public void UpdateData(FileDirectory fileDirectory) {
            Log = fileDirectory.Log??Log;
            Cookie = fileDirectory.Cookie??Cookie;
        }
        public void TryToResetDefault(){
            if (string.IsNullOrEmpty(Root)) { Root = AppDomain.CurrentDomain.BaseDirectory + @"AppData\"; }
            if (string.IsNullOrEmpty(Log)) { Log = Root + @"Logs\"; }
            if (string.IsNullOrEmpty(Cookie)) { Cookie = Root + @"Cookie\"; }
        }
        public void ResetDefault(string name) {
            switch(name.ToLower()) {
                case "log":
                    Log = Root + @"Logs\";
                    break;
                case "cookie":
                    Cookie = Root + @"Cookies\";
                    break;
                default:
                    break;
            }
        }
    }
}