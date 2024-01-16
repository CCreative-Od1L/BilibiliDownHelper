using System.Text.Json.Serialization;
using Core.PleInterface;

namespace Core.BilibiliApi.User;

public class LoginUserInfoResponse : BaseResponse<LoginUserInfoData>{
    public override bool IsValid() {
        return Code.Equals(0);
    }
}