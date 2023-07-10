using ShopPhone.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response
{
    public class ProductoDTO 
    {
        public int IdProducto { get; set; }
        public string Descripcion { get; set; } = "";
        public int IdCategoria { get; set; }
        public int  Inventario { get; set; }
        public double PrecioUnitario { get; set; }
        public bool Estado { set; get; } = true;
        public string URLImagen { set; get; } = "";
        public string Comentarios { set; get; } = "";
        public string? Base64Image { get; set; }
        public string? FileName { get; set; }

        public CategoriaDTO _Categoria { get; set; } = new CategoriaDTO();

    }
}
