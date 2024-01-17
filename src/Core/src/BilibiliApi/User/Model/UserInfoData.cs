using System.Text.Json.Serialization;

namespace Core.BilibiliApi.User.Model;
public class UserInfoData {
    [JsonPropertyName("mid")]
    public int Mid { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("sex")]
    public string? Sex { get; set; }
    [JsonPropertyName("face")]      // * 头像链接
    public string? Face { get; set; }
    [JsonPropertyName("is_followed")]
    public bool IsFollowed { get; set; }
    [JsonPropertyName("is_senior_member")]  // * 是否为硬核会员(0：否，1：是)
    public int IsSeniorMember { get; set; }
}