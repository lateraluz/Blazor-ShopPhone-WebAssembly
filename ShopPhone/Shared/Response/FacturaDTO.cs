using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public  class FacturaDTO
{
    public int IdFactura { get; set; } = 0;
    public int IdCliente { get; set; }
    public DateTime FechaVenta { get; set; } = DateTime.Now;
    public List<FacturaDetalleDTO> _DetalleFactura { get; set; } = new List<FacturaDetalleDTO>();

}

