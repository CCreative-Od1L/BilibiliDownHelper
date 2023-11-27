using System.Text;

namespace Core.Cookie {
    public class CookieData {
        public string CookieName;
        public Dictionary<string, string> KeyValuePairs;
        public List<string> Attribute;

        public CookieData(string cookieText) {
            CookieName = string.Empty;
            KeyValuePairs = [];
            Attribute = [];

            foreach(var part in cookieText.Split(';')) {
                if (part.Contains('=')) {
                    var parts = part.Split('=', 2);
                    if (string.IsNullOrEmpty(CookieName)) {
                        CookieName = parts[0].Trim();
                    }
                    KeyValuePairs.Add(parts[0].Trim(), parts[1].Trim());
                } else {
                    Attribute.Add(part.Trim());
                }
            }
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Cookie Name: " + CookieName);
            foreach(var pair in KeyValuePairs) {
                stringBuilder.AppendLine(string.Format("{0} := {1}", pair.Key, pair.Value));
            }
            foreach(var attr in Attribute) {
                stringBuilder.AppendLine("Attribute: " + attr);
            }
            return stringBuilder.ToString();
        }
        public string TryToGetValue(string key) {
            return KeyValuePairs.GetValueOrDefault(key, string.Empty);
        }

        public List<string> GetAttribute() {
            return Attribute;
        }
    }
}