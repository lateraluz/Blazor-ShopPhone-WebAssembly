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
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<Producto, ProductoDTO>();
            CreateMap<ProductoDTO, Producto>();
            
            CreateMap<Producto, ProductoDTO>()
               .ForMember(dest => dest.IdProducto, orig => orig.MapFrom(x => x.IdProducto))
               .ForMember(dest => dest.Descripcion, orig => orig.MapFrom(x => x.Descripcion.Trim()))
               .ForMember(dest => dest.IdCategoria, orig => orig.MapFrom(x => x.IdCategoria))
               .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado))
               .ForMember(dest => dest.Inventario, orig => orig.MapFrom(x => x.Inventario))
               .ForMember(dest => dest.Comentarios, orig => orig.MapFrom(x => x.Comentarios!.Trim()))
               .ForMember(dest => dest.PrecioUnitario, orig => orig.MapFrom(x => x.PrecioUnitario))
               .ForMember(dest => dest.URLImagen, orig => orig.MapFrom(x => x.Urlimagen!.Trim()))
               .ForMember(dest => dest._Categoria, orig => orig.MapFrom(x => x.IdCategoriaNavigation));


            CreateMap<ProductoDTO, Producto>()
               .ForMember(dest => dest.IdProducto, orig => orig.MapFrom(x => x.IdProducto))
               .ForMember(dest => dest.Descripcion, orig => orig.MapFrom(x => x.Descripcion.Trim()))
               .ForMember(dest => dest.IdCategoria, orig => orig.MapFrom(x => x.IdCategoria))
               .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado))
               .ForMember(dest => dest.Inventario, orig => orig.MapFrom(x => x.Inventario))
               .ForMember(dest => dest.Comentarios, orig => orig.MapFrom(x => x.Comentarios.Trim()))
               .ForMember(dest => dest.PrecioUnitario, orig => orig.MapFrom(x => x.PrecioUnitario))
               .ForMember(dest => dest.Urlimagen, orig => orig.MapFrom(x => x.URLImagen.Trim()));
               
        }

    }
}
