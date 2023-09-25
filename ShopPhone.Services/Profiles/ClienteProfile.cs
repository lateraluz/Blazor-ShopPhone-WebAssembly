using AutoMapper;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopPhone.Services.Mappers;

public class ClienteProfile : Profile
{
    public ClienteProfile()
    {
        CreateMap<Cliente, ClienteDTO>();
        CreateMap<ClienteDTO, Cliente>();
        CreateMap<Cliente, ClienteDTO>()
            .ForMember(dest => dest.IdCliente, orig => orig.MapFrom(x => x.IdCliente))
            .ForMember(dest => dest.Nombre, orig => orig.MapFrom(x => x.Nombre.Trim()))
            .ForMember(dest => dest.Apellidos, orig => orig.MapFrom(x => x.Apellidos.Trim()))
            .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado))
            .ForMember(dest => dest.Telefono, orig => orig.MapFrom(x => x.Telefono.Trim()))
            .ForMember(dest => dest.CorreoElectronico, orig => orig.MapFrom(x => x.CorreoElectronico.Trim()))
            .ForMember(dest => dest.FechaNacimiento, orig => orig.MapFrom(x => x.FechaNacimiento))
            .ForMember(dest => dest.LastUpdate, orig => orig.MapFrom(x => x.LastUpdate));
        CreateMap<ClienteDTO, Cliente>()
           .ForMember(dest => dest.IdCliente, orig => orig.MapFrom(x => x.IdCliente))
           .ForMember(dest => dest.Nombre, orig => orig.MapFrom(x => x.Nombre.Trim()))
           .ForMember(dest => dest.Apellidos, orig => orig.MapFrom(x => x.Apellidos.Trim()))
           .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado))
           .ForMember(dest => dest.Telefono, orig => orig.MapFrom(x => x.Telefono.Trim()))
           .ForMember(dest => dest.CorreoElectronico, orig => orig.MapFrom(x => x.CorreoElectronico.Trim()))
           .ForMember(dest => dest.FechaNacimiento, orig => orig.MapFrom(x => x.FechaNacimiento))
           .ForMember(dest => dest.LastUpdate, orig => orig.MapFrom(x => x.LastUpdate));
    }


}
