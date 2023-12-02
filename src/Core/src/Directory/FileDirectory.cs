namespace Core.Directory {
    public class FileDirectory {
        public string Root {get; private set;}
        public string? Log {get; set;}
        public string? Cookie {get; set;}
        public string? Crypto {get; set;}
        public FileDirectory() {
            Root = AppDomain.CurrentDomain.BaseDirectory + @"AppData\";
        }
        public void UpdateData(FileDirectory fileDirectory) {
            Log = fileDirectory.Log??Log;
            Cookie = fileDirectory.Cookie??Cookie;
            Crypto = fileDirectory.Crypto??Crypto;
        }
        public void TryToResetDefault(){
            if (string.IsNullOrEmpty(Root)) { Root = AppDomain.CurrentDomain.BaseDirectory + @"AppData\"; }
            if (string.IsNullOrEmpty(Log)) { Log = Root + @"Logs\"; }
            if (string.IsNullOrEmpty(Cookie)) { Cookie = Root + @"Cookie\"; }
            if (string.IsNullOrEmpty(Crypto)) { Crypto = Root + @"Crypto\"; }
        }
        public void ResetDefault(string name) {
            switch(name.ToLower()) {
                case "log":
                    Log = Root + @"Logs\";
                    break;
                case "cookie":
                    Cookie = Root + @"Cookies\";
                    break;
                case "crypto":
                    Crypto = Root + @"Crypto\";
                    break;
                default:
                    break;
            }
        }
    }
}