using System;
using System.Collections.Generic;

namespace ShopPhone.DataAccess;

public partial class FacturaEncabezado
{
    public int IdFactura { get; set; }

    public DateTime FechaVenta { get; set; }

    public int IdCliente { get; set; }

    public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;
}
