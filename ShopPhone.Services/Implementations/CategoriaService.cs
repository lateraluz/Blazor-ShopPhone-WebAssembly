using AutoMapper;
using Microsoft.Extensions.Logging;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;
using System.Reflection;


namespace ShopPhone.Services.Implementations;

public class CategoriaService : ICategoriaService
{

    private ICategoriaRepository _categoriaRepository;
    private readonly IMapper _mapper;
    private ILogger<CategoriaService> _logger;

    public CategoriaService(ICategoriaRepository repository, IMapper mapper, ILogger<CategoriaService> logger)
    {
        _categoriaRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByDescriptionAsync(string description)
    {
        BaseResponseGeneric<ICollection<CategoriaDTO>> response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();

        try
        {
            var collection = await _categoriaRepository.FindByDescriptionAsync(description);

            response.Success = true;
            response.Data = _mapper.Map<ICollection<CategoriaDTO>>(collection);

            return response;
        }
        catch (Exception e)
        {
            Exception ex = e;
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }


    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> ListAsync()
    {
        BaseResponseGeneric<ICollection<CategoriaDTO>> response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();

        try
        {
            var collection = await _categoriaRepository.ListAsync();

            response.Success = true;
            response.Data = _mapper.Map<ICollection<CategoriaDTO>>(collection);

            return response;
        }
        catch (Exception e)
        {
            Exception ex = e;
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(CategoriaDTO identitiy)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var @object = _mapper.Map<Categorium>(identitiy);

            response.Data = await _categoriaRepository.AddAsync(@object);
            response.Success = true;

            _logger.LogInformation("Categoria agregado con exito");

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "Error al Agregar el Categoria";
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
        }


    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {

        var response = new BaseResponse();
        try
        {
            await _categoriaRepository.DeleteAsync(id);
            response.Success = true;
            return response;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al Eliminar";
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByIdAsync(int id)
    {
        BaseResponseGeneric<ICollection<CategoriaDTO>> response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();

        try
        {
            var entity = await _categoriaRepository.FindAsync(id);

            response.Success = true;
            var @object = _mapper.Map<CategoriaDTO>(entity);
            if (@object != null)
            {
                List<CategoriaDTO> lista = new List<CategoriaDTO>();
                lista.Add(@object);
                response.Data = lista;
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, CategoriaDTO request)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _categoriaRepository.FindAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro la categoria";
                return response;
            }

            // Request va a reemplazar todos los valores coincidentes en el objeto de destino
            // que se encuentra en el lado derecho
            _mapper.Map(request, entity);

            await _categoriaRepository.UpdateAsync();
            response.Success = true;
            return response;

        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al Actualizar el Genero";
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return response;
        }


    }

}
