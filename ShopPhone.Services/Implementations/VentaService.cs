using AutoMapper;
using Azure;
using log4net;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Implementations;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Implementations
{
    public class VentaService : IVentaService
    {
        private IVentaRepository _VentaRepository;
        private readonly IMapper _Mapper;
        private ILog _Logger;


        public VentaService(IVentaRepository repository, IMapper mapper, ILog logger)
        {
            _VentaRepository = repository;
            _Mapper = mapper;
            _Logger = logger;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(FacturaDTO identity)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {

                // Map
                var factura = _Mapper.Map<FacturaEncabezado>(identity);

                // Get No Receipt and assign Receipt Number
                factura.IdFactura = _VentaRepository.GetNoReceipt();

                identity._FacturaDetalle.ForEach(
                    x => x.IdFactura = identity.IdFactura
                    );

                var baseResponse = await _VentaRepository.AddAsync(factura);
                response.Success = true;
                response.Data = identity.IdFactura;
                _Logger.Info($"Venta realizada con exito");
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = $"Error al salvar el factura ";
                _Logger.Error($"{response.ErrorMessage}  en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                return response;
            }
        }


        public Task<BaseResponseGeneric<ICollection<FacturaDTO>>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateAsync(int id, FacturaDTO request)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponseGeneric<ICollection<FacturaDTO>>> ListAsync()
        {
            BaseResponseGeneric<ICollection<FacturaDTO>> response = new BaseResponseGeneric<ICollection<FacturaDTO>>();

            try
            {
               
                var collection = await _VentaRepository.ListAsync();

                response.Success = true;
                response.Data = _Mapper.Map<ICollection<FacturaDTO>>(collection);

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = $"Error al consultar las facturas ";
                _Logger.Error($"{response.ErrorMessage}  en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                return response;
            }
        }
    }
}
