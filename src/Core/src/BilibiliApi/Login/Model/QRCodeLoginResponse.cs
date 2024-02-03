using System.Text.Json.Serialization;
using Core.PleInterface;
using Core.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;
/// <summary>
/// * 登录结果返回 json 对象
/// </summary>
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
    public class QRCodeLoginResponse : BaseResponse<LoginResponseData> {
        public override bool IsValid() {
            return base.IsValid();
        }
        public QRCODE_SCAN_STATUS GetQRCodeStatus() {
            if (Data == null) { return QRCODE_SCAN_STATUS.NOT_SCAN; }
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
        public string Url { get; set; } = string.Empty;
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        public DateTime GetLoginTime() {
            return new DateTime(Timestamp);
        }
    }
}