using System.Text.Json.Serialization;

namespace Core.BilibiliApi.User {
    public class LoginUserInfoData {
        [JsonPropertyName("isLogin")]
        public bool IsLogin { get; set; }
        [JsonPropertyName("email_verified")]
        public int EmailVerified { get; set; }
        [JsonPropertyName("face")]
        public string? Face { get; set; }
        [JsonPropertyName("level_info")]
        public LevelInfoSuc? LevelInfo { get; set; }
        public class LevelInfoSuc {
            [JsonPropertyName("current_level")]
            public int CurrentLevel { get; set; }
        }
        [JsonPropertyName("mid")]
        public long Mid { get; set; }
        [JsonPropertyName("uname")]
        public string? Uname { get; set; }
        [JsonPropertyName("is_senior_member")] 
        public int IsSeniorMember { get; set; }
        public int GetUserLevel() {
            if (LevelInfo != null) {
                return LevelInfo.CurrentLevel;
            }
            return -1;
        }
    }
}