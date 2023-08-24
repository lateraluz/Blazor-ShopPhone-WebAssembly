using AutoMapper;
using log4net;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Implementations;

public class ClienteService : IClienteService
{
    private IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;
    private ILog _logger;

    public ClienteService(IClienteRepository repository, IMapper mapper, ILog logger)
    {
        _clienteRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponseGeneric<ICollection<ClienteDTO>>> FindByDescriptionAsync(string description)
    {
        BaseResponseGeneric<ICollection<ClienteDTO>> response = new BaseResponseGeneric<ICollection<ClienteDTO>>();

        try
        {
            var collection = await _clienteRepository.FindByDescriptionAsync(description);

            response.Success = true;
            response.Data = _mapper.Map<ICollection<ClienteDTO>>(collection);

            return response;
        }
        catch (Exception e)
        {
            Exception ex = e;
            _logger.Error(ex);
            throw;
        }
    }


    public async Task<BaseResponseGeneric<ICollection<ClienteDTO>>> ListAsync()
    {
        BaseResponseGeneric<ICollection<ClienteDTO>> response = new BaseResponseGeneric<ICollection<ClienteDTO>>();

        try
        {
            var collection = await _clienteRepository.ListAsync();

            response.Success = true;
            response.Data = _mapper.Map<ICollection<ClienteDTO>>(collection);

            return response;
        }
        catch (Exception e)
        {
            Exception ex = e;
            _logger.Error(ex);
            throw;
        }
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(ClienteDTO identitiy)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var @object = _mapper.Map<Cliente>(identitiy);
            response.Data = await _clienteRepository.AddAsync(@object);
            response.Success = true;

            _logger.Info("Cliente agregado con exito");

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "Error al Agregar el Cliente";
            _logger.Error(ex.Message);
            return response;
        }


    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {

        var response = new BaseResponse();
        try
        {
            await _clienteRepository.DeleteAsync(id);
            response.Success = true;
            return response;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al Eliminar";
            _logger.Error(ex.Message);
            return response;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<ClienteDTO>>> FindByIdAsync(int id)
    {
        BaseResponseGeneric<ICollection<ClienteDTO>> response = new BaseResponseGeneric<ICollection<ClienteDTO>>();

        try
        {
            var entity = await _clienteRepository.FindAsync(id);

            response.Success = true;
            var @object = _mapper.Map<ClienteDTO>(entity);
            List<ClienteDTO> lista = new List<ClienteDTO>();
            lista.Add(@object);
            response.Data = lista;

            return response;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw;
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, ClienteDTO request)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _clienteRepository.FindAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el Cliente";
                return response;
            }

            // Request va a reemplazar todos los valores coincidentes en el objeto de destino
            // que se encuentra en el lado derecho
            _mapper.Map(request, entity);

            await _clienteRepository.UpdateAsync();
            response.Success = true;
            return response;

        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al Actualizar el Cliente";
            _logger.Error(ex.Message);
            return response;
        }


    }

}

