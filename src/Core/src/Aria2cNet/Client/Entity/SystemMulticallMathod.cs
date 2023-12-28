using System.Text.Json.Serialization;

namespace Core.Aria2cNet.Client.Entity;
public class SystemMulticallMathod {
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("params")]
    public List<object>? Params { get; set; }
}