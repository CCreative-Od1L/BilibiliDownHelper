using System.Text.Json.Serialization;
using Core.PleInterface;

namespace Core.BilibiliApi.Video.Model;
public class VideoStreamResponse :BaseResponse<VideoStreamData> {
    public override bool IsValid() {
        return base.IsValid();
    }
}

public class VideoStreamData {
    [JsonPropertyName("quality")]
    public int Quality { get; set; }

}