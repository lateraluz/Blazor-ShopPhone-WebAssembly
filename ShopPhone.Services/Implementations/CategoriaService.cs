using AutoMapper;
using Azure.Core;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Implementations;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Implementations;

public class CategoriaService : ICategoriaService
{

    private ICategoriaRepository _CategoriaRepository;
    private readonly IMapper _Mapper;
    private ILog _Logger;

    public CategoriaService(ICategoriaRepository repository, IMapper mapper, ILog logger)
    {
        _CategoriaRepository = repository;
        _Mapper = mapper;
        _Logger = logger;
    }

    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByDescriptionAsync(string description)
    {
        BaseResponseGeneric<ICollection<CategoriaDTO>> response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();

        try
        {
            var collection = await _CategoriaRepository.FindByDescriptionAsync(description);

            response.Success = true;
            response.Data = _Mapper.Map<ICollection<CategoriaDTO>>(collection);

            return response;
        }
        catch (Exception e)
        {
            Exception ex = e;
            _Logger.Error(ex);
            throw;
        }
    }


    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> ListAsync()
    {
        BaseResponseGeneric<ICollection<CategoriaDTO>> response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();

        try
        {
            var collection = await _CategoriaRepository.ListAsync();

            response.Success = true;
            response.Data = _Mapper.Map<ICollection<CategoriaDTO>>(collection);

            return response;
        }
        catch (Exception e)
        {
            Exception ex = e;
            _Logger.Error(ex);
            throw;
        }
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(CategoriaDTO identitiy)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var @object = _Mapper.Map<Categorium>(identitiy);
            response.Data = await _CategoriaRepository.AddAsync(@object);
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
            await _CategoriaRepository.DeleteAsync(id);
            response.Success = true;
            return response;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al Eliminar";
            _Logger.Error(ex.Message);
            return response;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByIdAsync(int id)
    {
        BaseResponseGeneric<ICollection<CategoriaDTO>> response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();

        try
        {
            var entity = await _CategoriaRepository.FindAsync(id);

            response.Success = true;
            var @object = _Mapper.Map<CategoriaDTO>(entity);
            List<CategoriaDTO> lista = new List<CategoriaDTO>();
            lista.Add(@object);
            response.Data = lista;

            return response;
        }
        catch (Exception ex)
        {
            _Logger.Error(ex.Message);
            throw;
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, CategoriaDTO request)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _CategoriaRepository.FindAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro la categoria";
                return response;
            }

            // Request va a reemplazar todos los valores coincidentes en el objeto de destino
            // que se encuentra en el lado derecho
            _Mapper.Map(request, entity);

            await _CategoriaRepository.UpdateAsync();
            response.Success = true;
            return response;

        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al Actualizar el Genero";
            _Logger.Error(ex.Message);
            return response;
        }


    }

}
