using AutoMapper;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Mappers
{
    public class MapperFactura : Profile
    {
        public MapperFactura()
        {
            CreateMap<FacturaDTO, FacturaEncabezado>();
            CreateMap<FacturaDetalleDTO, FacturaDetalle>();
            CreateMap<FacturaDetalle, FacturaDetalleDTO>();

            CreateMap<FacturaDTO, FacturaEncabezado>()
            .ForMember(dest => dest.IdFactura, orig => orig.MapFrom(x => x.IdFactura))
            .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado))
            .ForMember(dest => dest.FechaVenta, orig => orig.MapFrom(x => x.FechaVenta))
            .ForMember(dest => dest.IdCliente, orig => orig.MapFrom(x => x.IdCliente))
           // .ForMember(dest => dest.IdClienteNavigation, orig => orig.MapFrom(x => x._Cliente))
            .ForMember(dest => dest.FacturaDetalles, orig => orig.MapFrom(x => x._FacturaDetalle));


            CreateMap<FacturaEncabezado, FacturaDTO>()
           .ForMember(dest => dest.IdFactura, orig => orig.MapFrom(x => x.IdFactura))
           .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado))
           .ForMember(dest => dest.FechaVenta, orig => orig.MapFrom(x => x.FechaVenta))
           .ForMember(dest => dest.IdCliente, orig => orig.MapFrom(x => x.IdCliente))
           .ForMember(dest => dest._Cliente, orig => orig.MapFrom(x => x.IdClienteNavigation))
           .ForMember(dest => dest._FacturaDetalle, orig => orig.MapFrom(x => x.FacturaDetalles));

        }
    }
}
