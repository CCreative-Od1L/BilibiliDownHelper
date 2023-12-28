using System.Text.Json.Serialization;
using Core.Utils;

namespace Core.Aria2cNet.Client.Entity;
public class AriaTellStatus {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }

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
    public string? Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string? Jsonrpc { get; set; }

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
    public string? Bitfield { get; set; }

    [JsonPropertyName("completedLength")]
    public string? CompletedLength { get; set; }

    [JsonPropertyName("connections")]
    public string? Connections { get; set; }

    [JsonPropertyName("dir")]
    public string? Dir { get; set; }

    [JsonPropertyName("downloadSpeed")]
    public string? DownloadSpeed { get; set; }

    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("files")]
    public List<AriaTellStatusResultFile>? Files { get; set; }

    [JsonPropertyName("gid")]
    public string? Gid { get; set; }

    [JsonPropertyName("numPieces")]
    public string? NumPieces { get; set; }

    [JsonPropertyName("pieceLength")]
    public string? PieceLength { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("totalLength")]
    public string? TotalLength { get; set; }

    [JsonPropertyName("uploadLength")]
    public string? UploadLength { get; set; }

    [JsonPropertyName("uploadSpeed")]
    public string? UploadSpeed { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}
public class AriaTellStatusResultFile {
    [JsonPropertyName("completedLength")]
    public string? CompletedLength { get; set; }

    [JsonPropertyName("index")]
    public string? Index { get; set; }

    [JsonPropertyName("length")]
    public string? Length { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("selected")]
    public string? Selected { get; set; }

    [JsonPropertyName("uris")]
    public List<AriaUri>? Uris { get; set; }

    public override string ToString() {
        return JsonUtils.SerializeJsonObj(this);
    }
}