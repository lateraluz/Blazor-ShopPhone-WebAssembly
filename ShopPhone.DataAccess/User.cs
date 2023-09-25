using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopPhone.DataAccess;

public partial class User
{
    public string Login { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string IdRol { get; set; } = null!;

    public bool Estado { get; set; }
    [ConcurrencyCheck]
    public DateTime? LastUpdate { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
