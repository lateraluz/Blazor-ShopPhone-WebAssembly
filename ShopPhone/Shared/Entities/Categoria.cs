using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Entities;

public class Categoria :EntityBase
{
    public int IdCategoria { get; set; } = 0;
    public string NombreCategoria { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;

}