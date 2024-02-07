using System.Text.Json.Serialization;

namespace Core.BilibiliApi.User {
    public class LoginUserInfoData {
        [JsonPropertyName("isLogin")]
        public bool IsLogin { get; set; }       // * 是否登录
        [JsonPropertyName("email_verified")]
        public int EmailVerified { get; set; }  // * 是否验证了邮箱
        [JsonPropertyName("face")]
        public string? Face { get; set; }       // * 头像Url
        [JsonPropertyName("level_info")]
        public LevelInfoSuc? LevelInfo { get; set; }    // * 等级信息
        public class LevelInfoSuc {
            [JsonPropertyName("current_level")]
            public int CurrentLevel { get; set; }       // * 当前等级
            [JsonPropertyName("current_min")]
            public int CurrentMin { get; set; }         // * 当前等级的最小经验值
            [JsonPropertyName("current_exp")]
            public int CurrentExp { get; set; }         // * 当前的经验值
        }
        [JsonPropertyName("mid")]
        public long Mid { get; set; }       // * uid
        [JsonPropertyName("uname")]
        public string? Uname { get; set; }  // * 用户名称
        [JsonPropertyName("vipStatus")]
        public int VipStatus { get; set; }  // * VIP状态
        [JsonPropertyName("is_senior_member")] 
        public int IsSeniorMember { get; set; } // * 是否是硬核会员
        public int GetUserLevel() {
            if (LevelInfo != null) {
                return LevelInfo.CurrentLevel;
            }
            return -1;
        }
        public bool IsVIP() {
            return VipStatus != 0;
        }
    }
}