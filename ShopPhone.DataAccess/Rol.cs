using System;
using System.Collections.Generic;

namespace ShopPhone.DataAccess;

public partial class Rol
{
    public string IdRol { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
