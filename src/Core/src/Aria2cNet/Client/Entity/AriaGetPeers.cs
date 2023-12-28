using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
public class AriaGetPeers {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }

    [JsonPropertyName("result")]
    public List<AriaPeer>? Result { get; set; }

    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}

public class AriaPeer {
    [JsonPropertyName("amChoking")]
    public string? AmChoking { get; set; }

    [JsonPropertyName("bitfield")]
    public string? Bitfield { get; set; }

    [JsonPropertyName("downloadSpeed")]
    public string? DownloadSpeed { get; set; }

    [JsonPropertyName("ip")]
    public string? Ip { get; set; }

    [JsonPropertyName("peerChoking")]
    public string? PeerChoking { get; set; }

    [JsonPropertyName("peerId")]
    public string? PeerId { get; set; }

    [JsonPropertyName("port")]
    public string? Port { get; set; }

    [JsonPropertyName("seeder")]
    public string? Seeder { get; set; }

    [JsonPropertyName("uploadSpeed")]
    public string? UploadSpeed { get; set; }

    public override string? ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
