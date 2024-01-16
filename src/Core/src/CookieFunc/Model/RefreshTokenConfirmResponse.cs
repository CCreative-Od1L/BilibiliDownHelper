using System.Text.Json.Serialization;
using Core.PleInterface;

namespace Core.CookieFunc.Model;

public class RefreshTokenConfirmResponse : BaseResponse<object>{
    public override bool IsValid() {
        return base.IsValid();
    }
}