using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public class ClienteDTO
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = "";
    public string Apellidos { get; set; } = "";
}
