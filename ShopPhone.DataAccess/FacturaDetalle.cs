using System;
using System.Collections.Generic;

namespace ShopPhone.DataAccess;

public partial class FacturaDetalle
{
    public int Secuencia { get; set; }

    public int IdFactura { get; set; }

    public int IdProducto { get; set; }

    public decimal PrecioUnitario { get; set; }

    public int Cantidad { get; set; }

    public decimal Impuesto { get; set; }

    public virtual FacturaEncabezado IdFacturaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
