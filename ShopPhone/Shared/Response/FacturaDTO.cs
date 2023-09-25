using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public  record FacturaDTO
{
    public int IdFactura { get; set; } = 0;
    public int IdCliente { get; set; }
    public ClienteDTO _Cliente { get; set; }= new ClienteDTO();
    public bool Estado { get; set; }
    public DateTime FechaVenta { get; set; } = DateTime.Now;
    public List<FacturaDetalleDTO> _FacturaDetalle { get; set; } = new List<FacturaDetalleDTO>();
    public DateTime LastUpdate { set; get; } = DateTime.Now;
}

