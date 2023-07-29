using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.DataAccess;


public class ShopPhoneUserIdentity : IdentityUser
{
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = default!;

    [StringLength(20)]
    public string DocumentNumber { get; set; } = default!;   
 
}