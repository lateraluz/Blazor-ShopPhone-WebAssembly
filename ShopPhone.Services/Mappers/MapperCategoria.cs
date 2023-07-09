using AutoMapper;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Mappers;

public class MapperCategoria : Profile
{
    public MapperCategoria()
    {
        CreateMap<Categorium, CategoriaDTO>();

        CreateMap<Categorium, CategoriaDTO>()
            .ForMember(dest => dest.IdCategoria, orig => orig.MapFrom(x => x.IdCategoria))
            .ForMember(dest => dest.NombreCategoria, orig => orig.MapFrom(x => x.NombreCategoria.Trim()))
            .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado));

        CreateMap<CategoriaDTO, Categorium>()
          .ForMember(dest => dest.IdCategoria, orig => orig.MapFrom(x => x.IdCategoria))
          .ForMember(dest => dest.NombreCategoria, orig => orig.MapFrom(x => x.NombreCategoria.Trim()))
          .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado));

    }
}
