using System;
using System.Collections.Generic;

namespace ShopPhone.DataAccess;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Telefono { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<FacturaEncabezado> FacturaEncabezados { get; set; } = new List<FacturaEncabezado>();
}
