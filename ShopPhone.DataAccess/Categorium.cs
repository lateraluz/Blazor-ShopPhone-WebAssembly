using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopPhone.DataAccess;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public bool Estado { get; set; }
    [ConcurrencyCheck]
    public DateTime? LastUpdate { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
