using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
public class AriaTellStatus {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("jsonrpc")]
    public string Jsonrpc { get; set; } = string.Empty;

    [JsonPropertyName("result")]
    public AriaTellStatusResult? Result { get; set; }

    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaTellStatusList {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("jsonrpc")]
    public string Jsonrpc { get; set; } = string.Empty;

    [JsonPropertyName("result")]
    public List<AriaTellStatusResult>? Result { get; set; }

    [JsonPropertyName("error")]
    public AriaError? Error { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaTellStatusResult {
    [JsonPropertyName("bitfield")]
    public string Bitfield { get; set; } = string.Empty;

    [JsonPropertyName("completedLength")]
    public string CompletedLength { get; set; } = string.Empty;

    [JsonPropertyName("connections")]
    public string Connections { get; set; } = string.Empty;

    [JsonPropertyName("dir")]
    public string Dir { get; set; } = string.Empty;

    [JsonPropertyName("downloadSpeed")]
    public string DownloadSpeed { get; set; } = string.Empty;

    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; set; } = string.Empty;

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    [JsonPropertyName("files")]
    public List<AriaTellStatusResultFile> Files { get; set; } = [];

    [JsonPropertyName("gid")]
    public string Gid { get; set; } = string.Empty;

    [JsonPropertyName("numPieces")]
    public string NumPieces { get; set; } = string.Empty;

    [JsonPropertyName("pieceLength")]
    public string PieceLength { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("totalLength")]
    public string TotalLength { get; set; } = string.Empty;

    [JsonPropertyName("uploadLength")]
    public string UploadLength { get; set; } = string.Empty;

    [JsonPropertyName("uploadSpeed")]
    public string UploadSpeed { get; set; } = string.Empty;

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaTellStatusResultFile {
    [JsonPropertyName("completedLength")]
    public string CompletedLength { get; set; } = string.Empty;

    [JsonPropertyName("index")]
    public string Index { get; set; } = string.Empty;

    [JsonPropertyName("length")]
    public string Length { get; set; } = string.Empty;

    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;

    [JsonPropertyName("selected")]
    public string Selected { get; set; } = string.Empty;

    [JsonPropertyName("uris")]
    public List<AriaUri>? Uris { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}