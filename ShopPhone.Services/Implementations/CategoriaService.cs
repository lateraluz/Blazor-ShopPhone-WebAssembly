using AutoMapper;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
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
    private ILog Logger;
    public CategoriaService(ICategoriaRepository repository, IMapper mapper, ILog logger)
    {
        _CategoriaRepository = repository;
        _Mapper = mapper;
        Logger = logger;
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
            Logger.Error(ex);
            throw;
        }
    }

}
