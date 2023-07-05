using AutoMapper;
using Azure.Core;
using log4net;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Implementations;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Implementations;

public class ProductoService : IProductoService
{

    private IProductoRepository _ProductoRepository;
    private readonly IMapper _Mapper;
    private ILog _Logger;
    private IFileUploader _FileUploader;

    public ProductoService(IProductoRepository repository, IMapper mapper, ILog logger, IFileUploader fileUploader)
    {
        _ProductoRepository = repository;
        _Mapper = mapper;
        _Logger = logger;
        _FileUploader = fileUploader;
    }

    public async Task<BaseResponseGeneric<ICollection<ProductoDTO>>> FindByDescriptionAsync(string description)
    {
        BaseResponseGeneric<ICollection<ProductoDTO>> response = new BaseResponseGeneric<ICollection<ProductoDTO>>();

        try
        {
            var collection = await _ProductoRepository.FindByDescriptionAsync(description);

            response.Success = true;
            response.Data = _Mapper.Map<ICollection<ProductoDTO>>(collection);

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = $"Error al buscar el Producto con al descripción {description}";
            _Logger.Error($"{response.ErrorMessage} en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
            throw;
        }
    }

    public async Task<BaseResponse> AddAsync(ProductoDTO identity)
    {
        BaseResponse response = new BaseResponse();
        try
        {
            identity.URLImagen = await _FileUploader.UploadFileAsync(identity.Base64Image, identity.FileName);

            var @object = _Mapper.Map<Producto>(identity);

            var baseResponse = await _ProductoRepository.AddAsync(@object);
            response.Success = true;
            response.ErrorMessage = baseResponse.ErrorMessage;
            _Logger.Info($"Producto agregado con exito");
            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = $"Error al Agregar el Producto {identity.IdProducto}- {identity.Descripcion}";
            _Logger.Error($"{response.ErrorMessage} {identity.IdProducto} {identity.Descripcion} en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
        }
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();
        try
        {
            await _ProductoRepository.DeleteAsync(id);
            response.Success = true;
            return response;
        }
        catch (Exception ex)
        {            
            response.Success = false;
            response.ErrorMessage = $"Error al eliminar el Producto {id}";
            _Logger.Error($"{response.ErrorMessage}  en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<ProductoDTO>>> FindByIdAsync(int id)
    {
        BaseResponseGeneric<ICollection<ProductoDTO>> response = new BaseResponseGeneric<ICollection<ProductoDTO>>();

        try
        {
            var entity = await _ProductoRepository.FindAsync(id);

            response.Success = true;
            var @object = _Mapper.Map<ProductoDTO>(entity);
            List<ProductoDTO> lista = new List<ProductoDTO>();
            lista.Add(@object);
            response.Data = lista;

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage =$"Error al Buscar  el Producto {id}";
            _Logger.Error($"{response.ErrorMessage}  en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, ProductoDTO identity)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _ProductoRepository.FindAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = $"No se encontro el Producto {id}";
                return response;
            }
             
            if (!string.IsNullOrEmpty(identity.Base64Image) == true)
                identity.URLImagen = await _FileUploader.UploadFileAsync(identity.Base64Image, identity.FileName);

            // Request va a reemplazar todos los valores coincidentes en el objeto de destino
            // que se encuentra en el lado derecho
            _Mapper.Map(identity, entity);

            await _ProductoRepository.UpdateAsync();
            response.Success = true;
            return response;

        }
        catch (Exception ex)
        {             
            response.Success = false;
            response.ErrorMessage = $"Error al Actualizar el Producto {id}";
            _Logger.Error($"{response.ErrorMessage}  en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
        }

    }


}
