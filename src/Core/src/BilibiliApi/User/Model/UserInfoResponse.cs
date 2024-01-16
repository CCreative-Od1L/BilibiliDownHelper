using Core.BilibiliApi.User.Model;
using Core.PleInterface;

namespace Core.BilibiliApi.Login.Model;

public class UserInfoResponse :BaseResponse<UserInfoData> {
    public override bool IsValid() {
        return base.IsValid();
    }
}