using System;
using System.Collections.Generic;

namespace ShopPhone.DataAccess;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdCategoria { get; set; }

    public int Inventario { get; set; }

    public decimal PrecioUnitario { get; set; }

    public bool Estado { get; set; }

    public string? Comentarios { get; set; }

    public string? Urlimagen { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;
}
