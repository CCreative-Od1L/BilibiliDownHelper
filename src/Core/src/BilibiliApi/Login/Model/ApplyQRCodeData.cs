using System.Diagnostics;
using System.Text.Json.Serialization;
using Core.PleInterface;
/// <summary>
/// * 二维码申请API 返回 json对象
/// </summary>
namespace Core.BilibiliApi.Login.Model {
    public class ApplyQRCodeData : BaseResponse<QRCodeData> {
        public override bool IsValid() {
            return base.IsValid();
        }
    }

    public class QRCodeData {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
        [JsonPropertyName("qrcode_key")]
        public string QRCodeKey { get; set; } = string.Empty;

        public bool IsValid() {
            return !string.IsNullOrEmpty(Url) && !string.IsNullOrEmpty(QRCodeKey);
        }
    }
}