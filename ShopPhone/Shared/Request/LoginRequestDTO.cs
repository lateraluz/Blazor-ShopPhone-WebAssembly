using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Request;

public class LoginRequestDTO
{
   
    public string UserName { get; set; } = default!;
 
    public string Password { get; set; } = default!;
}
