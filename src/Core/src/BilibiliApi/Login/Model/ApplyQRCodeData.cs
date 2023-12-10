using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Login.Model {
    public class ApplyQRCodeData {
        [JsonPropertyName("code")]
        public int Code {get; set;}
        [JsonPropertyName("message")]
        public string Message {get; set;}
        [JsonPropertyName("ttl")]
        public int TTL {get; set;}
        [JsonPropertyName("data")]
        public QRCodeData Data {get; set;}
    }

    public class QRCodeData {
        [JsonPropertyName("url")]
        public string Url {get; set;}
        [JsonPropertyName("qrcode_key")]
        public string QRCodeKey {get; set;}
    }
}