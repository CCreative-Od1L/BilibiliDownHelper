using System.Globalization;
using Core.Utils;

namespace Core.CookieFunc {
    public class CookieData {
        readonly System.Net.Cookie? _cookie;
        public System.Net.Cookie? Cookie {get => _cookie; }
        /// <summary>
        /// * 使用cookie响应数据整合到CookieData
        /// </summary>
        /// <param name="cookieText"></param>
        public CookieData(string cookieText) {
            _cookie = null;
            foreach(var part in cookieText.Split(';')) {
                if (part.Contains('=')) {
                    var parts = part.Split('=', 2);
                    if (_cookie == null) {
                        _cookie = new(parts[0].Trim(), parts[1].Trim());
                    } else {
                        SetCookieAttribute(parts[0].Trim(), parts[1].Trim());
                    }
                } else {
                    SetCookieAttribute(part.Trim());
                }
            }
        }
        /// <summary>
        /// * 设置Cookie属性
        /// </summary>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        void SetCookieAttribute(string attrName, string attrValue = "") {
            if (_cookie == null) { 
                CoreManager.logger.Error(nameof(SetCookieAttribute), "未初始化 _cookie 就修改属性。");
                return; 
            }
            switch(attrName) {
                case "Comment":
                    _cookie.Comment = attrValue;
                    break;
                case "CommentUri":
                    _cookie.CommentUri = new Uri(attrValue);
                    break;
                case "Discard":
                    _cookie.Discard = true;
                    break;
                case "Domain":
                    _cookie.Domain = attrValue;
                    break;
                case "Expired":
                    _cookie.Expired = true;
                    break;
                case "Expires":
                    if (attrValue.Contains("GMT")) {
                        attrValue = attrValue.Replace("GMT", "+0");
                    } else if (attrValue.Contains("UTC")) {
                        attrValue = attrValue.Replace("UTC", "");
                    }
                    _cookie.Expires = DateTimeUtils.TimeStringToDateTime(
                        attrValue,
                        "ddd, dd MMM yyyy HH:mm:ss z"
                    );
                    break;
                case "HttpOnly":
                    _cookie.HttpOnly = true;
                    break;
                case "Path":
                    _cookie.Path = attrValue;
                    break;
                case "Port":
                    _cookie.Port = attrValue;
                    break;
                case "Secure":
                    _cookie.Secure = true;
                    break;
                case "Version":
                    _cookie.Version = Convert.ToInt32(attrValue);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// * System.Net.Cookie - toString
        /// </summary>
        /// <returns></returns>
        public override string? ToString() {
            return _cookie?.ToString();
        }
    }
}