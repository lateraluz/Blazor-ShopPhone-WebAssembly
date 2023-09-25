using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public record ClienteDTO
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public bool Estado { get; set; } = default!;
    public DateTime FechaNacimiento { get; set; }  
    public string Telefono { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;

    public DateTime LastUpdate { set; get; } = DateTime.Now;
    public override string ToString()
    {
        return $"{IdCliente} - {Nombre} {Apellidos}";
    }

}
