using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Core.Utils {
    public class JsonUtils {
        static public T? ParseJsonString<T>(string jsonString) {
            return JsonSerializer.Deserialize<T>(jsonString);
        }
        static public string SerializeJsonObj<T>(T jsonObj, JsonSerializerOptions? options = null) {
            options ??= new JsonSerializerOptions() {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            };
            return JsonSerializer.Serialize(jsonObj, options);
        }
    }
}