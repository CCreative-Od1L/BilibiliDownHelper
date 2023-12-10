using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Login.Model {
    public enum QRCODE_SCAN_STATUS {
        // * 未扫码
        NOT_SCAN = 0,
        // * 成功
        SUCCESS,
        // * 二维码过期
        QRCODE_EXPRIED,
        // * 已扫描，未确认
        QRCODE_SCAN,
    }
    public class QRCodeLoginResponse {
        [JsonPropertyName("code")]
        public int Code {get; set;}
        [JsonPropertyName("message")]
        public string Message {get; set;}
        [JsonPropertyName("data")]
        public LoginResponseData Data {get; set;}

        public DateTime GetLoginTime() {
            return new DateTime(Data.Timestamp);
        }
        public QRCODE_SCAN_STATUS GetQRCodeStatus() {
            return Data.Code switch
            {
                0 => QRCODE_SCAN_STATUS.SUCCESS,
                86038 => QRCODE_SCAN_STATUS.QRCODE_EXPRIED,
                86090 => QRCODE_SCAN_STATUS.QRCODE_SCAN,
                _ => QRCODE_SCAN_STATUS.NOT_SCAN,
            };
        }
        public bool GetShouldWait() {
            if (GetQRCodeStatus() == QRCODE_SCAN_STATUS.SUCCESS 
            || GetQRCodeStatus() == QRCODE_SCAN_STATUS.QRCODE_EXPRIED) {
                return false;
            } else {
                return true;
            }
        }
    }

    public class LoginResponseData {
        [JsonPropertyName("url")]
        public string Url {get; set;}
        [JsonPropertyName("refresh_token")]
        public string RefreshToken {get; set;}
        [JsonPropertyName("timestamp")]
        public long Timestamp {get; set;}
        [JsonPropertyName("code")]
        public int Code {get; set;}
        [JsonPropertyName("message")]
        public string Message {get; set;}
    }
}