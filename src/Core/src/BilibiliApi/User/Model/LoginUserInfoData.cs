using System.Text.Json.Serialization;

namespace Core.BilibiliApi.User {
    public class LoginUserInfoData {
        [JsonPropertyName("isLogin")]
        public bool IsLogin { get; set; }       // * �Ƿ��¼
        [JsonPropertyName("email_verified")]
        public int EmailVerified { get; set; }  // * �Ƿ���֤������
        [JsonPropertyName("face")]
        public string? Face { get; set; }       // * ͷ��Url
        [JsonPropertyName("level_info")]
        public LevelInfoSuc? LevelInfo { get; set; }    // * �ȼ���Ϣ
        public class LevelInfoSuc {
            [JsonPropertyName("current_level")]
            public int CurrentLevel { get; set; }       // * ��ǰ�ȼ�
            [JsonPropertyName("current_min")]
            public int CurrentMin { get; set; }         // * ��ǰ�ȼ�����С����ֵ
            [JsonPropertyName("current_exp")]
            public int CurrentExp { get; set; }         // * ��ǰ�ľ���ֵ
        }
        [JsonPropertyName("mid")]
        public long Mid { get; set; }       // * uid
        [JsonPropertyName("uname")]
        public string? Uname { get; set; }  // * �û�����
        [JsonPropertyName("vipStatus")]
        public int VipStatus { get; set; }  // * VIP״̬
        [JsonPropertyName("is_senior_member")] 
        public int IsSeniorMember { get; set; } // * �Ƿ���Ӳ�˻�Ա
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