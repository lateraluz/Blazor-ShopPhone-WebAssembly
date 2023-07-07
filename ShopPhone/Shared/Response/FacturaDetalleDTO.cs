using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public class FacturaDetalleDTO
{
    public int IdProducto { get; set; }
    public string Descripcion { get; set; } = default!;
    public double PrecioUnitario { get; set; }
    public int Cantidad { get; set; }
    public int Impuesto { get; set; }

}