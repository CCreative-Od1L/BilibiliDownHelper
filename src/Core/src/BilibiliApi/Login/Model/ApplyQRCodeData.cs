using System.Diagnostics;

namespace Core.BilibiliApi.Login.Model {
    public class ApplyQRCodeData {
        public int code {get; set;}
        public string message {get; set;}
        public int ttl {get; set;}
        public QRCodeData data {get; set;}
    }

    public class QRCodeData {
        public string url {get; set;}
        public string qrcode_key {get; set;}
    }
}