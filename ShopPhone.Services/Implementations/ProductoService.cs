using AutoMapper;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace ShopPhone.Services.Implementations;

public class ProductoService : IProductoService
{

    private IProductoRepository _productoRepository;
    private readonly IMapper _mapper;
    private ILogger<ProductoService> _logger;
    private IFileUploader _fileUploader;

    public ProductoService(IProductoRepository repository, IMapper mapper, ILogger<ProductoService> logger, IFileUploader fileUploader)
    {
        _productoRepository = repository;
        _mapper = mapper;
        _logger = logger;
        _fileUploader = fileUploader;
    }

    public async Task<BaseResponseGeneric<ICollection<ProductoDTO>>> FindByDescriptionAsync(string description)
    {
        BaseResponseGeneric<ICollection<ProductoDTO>> response = new BaseResponseGeneric<ICollection<ProductoDTO>>();

        try
        {
            var collection = await _productoRepository.FindByDescriptionAsync(description);

            response.Success = true;
            response.Data = _mapper.Map<ICollection<ProductoDTO>>(collection);

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = $"Error al buscar el Producto con al descripción {description}";
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
            throw;
        }
    }

    public async Task<BaseResponse> AddAsync(ProductoDTO identity)
    {
        BaseResponse response = new BaseResponse();
        try
        {
            identity.URLImagen = await _fileUploader.UploadFileAsync(identity.Base64Image, identity.FileName);

            var @object = _mapper.Map<Producto>(identity);

            var baseResponse = await _productoRepository.AddAsync(@object);
            response.Success = true;
            response.ErrorMessage = baseResponse.ErrorMessage;
            _logger.LogInformation($"Producto agregado con exito");
            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = $"Error al Agregar el Producto {identity.IdProducto}- {identity.Descripcion}";
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
        }
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();
        try
        {
            await _productoRepository.DeleteAsync(id);
            response.Success = true;
            return response;
        }
        catch (Exception ex)
        {            
            response.Success = false;
            response.ErrorMessage = $"Error al eliminar el Producto {id}";
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<ProductoDTO>>> FindByIdAsync(int id)
    {
        BaseResponseGeneric<ICollection<ProductoDTO>> response = new BaseResponseGeneric<ICollection<ProductoDTO>>();

        try
        {
            var entity = await _productoRepository.FindAsync(id);

            response.Success = true;
            var @object = _mapper.Map<ProductoDTO>(entity);
            List<ProductoDTO> lista = new List<ProductoDTO>();
            lista.Add(@object);
            response.Data = lista;

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage =$"Error al Buscar  el Producto {id}";
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, ProductoDTO identity)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _productoRepository.FindAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = $"No se encontro el Producto {id}";
                return response;
            }
             
            if (!string.IsNullOrEmpty(identity.Base64Image) == true)
                identity.URLImagen = await _fileUploader.UploadFileAsync(identity.Base64Image, identity.FileName);

            // Request va a reemplazar todos los valores coincidentes en el objeto de destino
            // que se encuentra en el lado derecho
            _mapper.Map(identity, entity);

            await _productoRepository.UpdateAsync();
            response.Success = true;
            return response;

        }
        catch (Exception ex)
        {             
            response.Success = false;
            response.ErrorMessage = $"Error al Actualizar el Producto {id}";
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<ProductoDTO>>> ListAsync()
    {
        BaseResponseGeneric<ICollection<ProductoDTO>> response = new BaseResponseGeneric<ICollection<ProductoDTO>>();

        try
        {
            var collection = await _productoRepository.ListAsync();

            response.Success = true;
            response.Data = _mapper.Map<ICollection<ProductoDTO>>(collection);

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = $"Error al listar producto";
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }
}
