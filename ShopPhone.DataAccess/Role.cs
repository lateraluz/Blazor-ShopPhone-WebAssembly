using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopPhone.DataAccess;

public partial class Role
{
    
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
