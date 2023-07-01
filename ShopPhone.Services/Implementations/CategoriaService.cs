using AutoMapper;
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

            return await Task.FromResult(response);
        }
        catch (Exception e)
        {
            Exception ex = e;
            _Logger.Error(ex);
            throw;
        }
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(CategoriaDTO request)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var entity = _Mapper.Map<Categorium>(request);
            response.Data = await _CategoriaRepository.AddAsync(entity);
            response.Success = true;

            _Logger.Info("Genero agregado con exito");

            return response;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al Agregar el Genero";
            _Logger.Error(ex.Message);
            return response;
        }

       
    }
}
