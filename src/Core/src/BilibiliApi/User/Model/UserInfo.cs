using System.Text.Json.Serialization;

namespace Core.BilibiliApi.User {
    public class UserInfo {
        [JsonPropertyName("isLogin")]
        public bool IsLogin;
        [JsonPropertyName("email_verified")]
        public int EmailVerified;
        [JsonPropertyName("face")]
        public string? Face;
        [JsonPropertyName("level_info")]
        public LevelInfo? levelInfo;
        public class LevelInfo {
            [JsonPropertyName("current_level")]
            public int CurrentLevel;
        }
        [JsonPropertyName("uname")]
        public string? uname;
        [JsonPropertyName("is_senior_member")] 
        public int IsSeniorMember;
        public int GetUserLevel() {
            if (levelInfo != null) {
                return levelInfo.CurrentLevel;
            }
            return -1;
        }
    }
}