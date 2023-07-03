using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Mappers
{
    public class MapperProducto : Profile
    {
        public MapperProducto()
        {
            CreateMap<Producto, ProductoDTO>();

        }

    }
}
