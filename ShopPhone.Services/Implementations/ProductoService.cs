using AutoMapper;
using log4net;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Implementations;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Implementations;

public class ProductoService : IProductoService
{

    private IProductoRepository _ProductoRepository;
    private readonly IMapper _Mapper;
    private ILog _Logger;

    public ProductoService(IProductoRepository repository, IMapper mapper, ILog logger)
    {
        _ProductoRepository = repository;
        _Mapper = mapper;
        _Logger = logger;
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
        catch (Exception e)
        {
            Exception ex = e;
            _Logger.Error(ex);
            throw;
        }
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(ProductoDTO identitiy)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var @object = _Mapper.Map<Producto>(identitiy);
            response.Data = await _ProductoRepository.AddAsync(@object);
            response.Success = true;
            _Logger.Info("Categoria agregado con exito");
            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "Error al Agregar el Categoria";
            _Logger.Error(ex.Message);
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
            _Logger.Error(ex.Message);
            response.ErrorMessage = "Error al Eliminar";
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
        catch (Exception e)
        {
            Exception ex = e;
            _Logger.Error(ex);
            throw;
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, ProductoDTO request)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _ProductoRepository.FindAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro la categoria";
                return response;
            }

            // Request va a reemplazar todos los valores coincidentes en el objeto de destino
            // que se encuentra en el lado derecho
            _Mapper.Map(request, entity);

            await _ProductoRepository.UpdateAsync();
            response.Success = true;
            return response;

        }
        catch (Exception ex)
        {
            _Logger.Error(ex.Message);
            response.ErrorMessage = "Error al Actualizar el Genero";
            return response;
        }


    }

    
}
