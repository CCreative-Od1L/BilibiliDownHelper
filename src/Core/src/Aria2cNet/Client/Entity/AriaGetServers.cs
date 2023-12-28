using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
public class AriaGetServers {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }

    [JsonPropertyName("result")]
    public List<AriaGetServersResult>? Result { get; set; }

    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}

public class AriaGetServersResult {
    [JsonPropertyName("index")]
    public string? Index { get; set; }

    [JsonPropertyName("servers")]
    public List<AriaResultServer>? Servers { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaResultServer {
    [JsonPropertyName("currentUri")]
    public string? CurrentUri { get; set; }

    [JsonPropertyName("downloadSpeed")]
    public string? DownloadSpeed { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}