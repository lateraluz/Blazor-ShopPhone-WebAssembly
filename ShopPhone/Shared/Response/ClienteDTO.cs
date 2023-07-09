using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public class ClienteDTO
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = "";
    public string Apellidos { get; set; } = "";
    public bool Estado { get; set; } = default!;
    public DateTime FechaNacimiento { get; set; }  
    public string Telefono { get; set; } = "";
    public string CorreoElectronico { get; set; } = "";

    public override string ToString()
    {
        return $"{IdCliente} - {Nombre} {Apellidos}";
    }





}
