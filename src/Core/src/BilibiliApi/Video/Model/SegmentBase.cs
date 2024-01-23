using System.Text.Json.Serialization;

namespace Core.BilibiliApi.Video.Model;

public class SegmentBaseObj {
    // * ftyp box 加上 moov box 在 m4s 文件中的范围（单位为bytes）
    // * 例如：0-821表示开头820个字节
    [JsonPropertyName("initialization")]                
    public string Initialization { get; set; } = "";    // * ${init_first}-${init_last}
}