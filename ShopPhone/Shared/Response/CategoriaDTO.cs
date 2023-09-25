using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public record CategoriaDTO
{
    public int IdCategoria { set; get; }
    public string NombreCategoria { set; get; } = string.Empty;
    public bool Estado { set; get; }
};

/*
public class CategoriaDTO
{
    public int IdCategoria { get; set; } = 0;
    public string NombreCategoria { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;

}
*/