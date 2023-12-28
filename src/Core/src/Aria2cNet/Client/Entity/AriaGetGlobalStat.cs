using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
/*
     {
    "id": "qwer",
    "jsonrpc": "2.0",
    "result": {
        "downloadSpeed": "0",
        "numActive": "0",
        "numStopped": "0",
        "numStoppedTotal": "0",
        "numWaiting": "0",
        "uploadSpeed": "0"
    }
    }
     */
public class AriaGetGlobalStat {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }

    [JsonPropertyName("result")]
    public AriaGetGlobalStatResult? Result { get; set; }

    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaGetGlobalStatResult {
    [JsonPropertyName("downloadSpeed")]
    public string? DownloadSpeed { get; set; }

    [JsonPropertyName("numActive")]
    public string? NumActive { get; set; }

    [JsonPropertyName("numStopped")]
    public string? NumStopped { get; set; }

    [JsonPropertyName("numStoppedTotal")]
    public string? NumStoppedTotal { get; set; }

    [JsonPropertyName("numWaiting")]
    public string? NumWaiting { get; set; }

    [JsonPropertyName("uploadSpeed")]
    public string? UploadSpeed { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
