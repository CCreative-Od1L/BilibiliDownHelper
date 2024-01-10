using System.Text.Json;

namespace Core.Utils {
    public class JsonUtils {
        static public T? ParseJsonString<T>(string jsonString) {
            return JsonSerializer.Deserialize<T>(jsonString);
        }
        static public string SerializeJsonObj<T>(T jsonObj, JsonSerializerOptions? options = default) {
            return JsonSerializer.Serialize(jsonObj, options);
        }
    }
}