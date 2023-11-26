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
        public int code {get; set;}
        public string message {get; set;}
        public LoginResponseData data {get; set;}

        public DateTime GetLoginTime() {
            return new DateTime(data.timestamp);
        }
        public QRCODE_SCAN_STATUS GetQRCodeStatus() {
            return data.code switch
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
        public string url {get; set;}
        public string refresh_token {get; set;}
        public Int64 timestamp {get; set;}
        public int code {get; set;}
        public string message {get; set;}
    }
}