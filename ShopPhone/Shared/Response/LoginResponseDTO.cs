using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public class LoginResponseDTO :BaseResponse
{
    public string Token { get; set; } = default!;
    public int  Identificacion { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public List<string> Roles { get; set; } = default!;
}
