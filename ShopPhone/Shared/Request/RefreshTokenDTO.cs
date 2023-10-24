using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Request
{
    public class RefreshTokenDTO : BaseResponse
    {
        public string Token { get; set; } = default!;       

    }
}
