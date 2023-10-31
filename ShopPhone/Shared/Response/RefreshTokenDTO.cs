using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public class RefreshTokenDTO : BaseResponse
{
    public string Token { get; set; } = default!;
}
