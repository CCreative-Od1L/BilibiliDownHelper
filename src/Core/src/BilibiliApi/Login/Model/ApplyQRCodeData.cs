using System.Diagnostics;

namespace Core.BilibiliApi.Login.Model {
    public class ApplyQRCodeData {
        public int code {get; set;}
        public string message {get; set;}
        public int ttl {get; set;}
        public Data data {get; set;}
    }

    public class Data {
        public string url {get; set;}
        public string qrcode_key {get; set;}
    }
}