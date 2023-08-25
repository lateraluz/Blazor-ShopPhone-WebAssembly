using Microsoft.AspNetCore.Mvc;
using log4net;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using ShopPhone.Shared.Response;
using ShopPhone.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using MethodTimer;

namespace ShopPhone.Server.Controllers;

/// <summary>
/// Category using IMemoryCache just for practice
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriaController : ControllerBase
{
    private IMemoryCache _cache;
    private ICategoriaService _categoriaService;
    private ILog _logger;
    private MemoryCacheEntryOptions _cacheEntryOptions;
    public CategoriaController(ICategoriaService categoriaService, ILog logger, IMemoryCache cache)
    {
        _categoriaService = categoriaService;
        _logger = logger;
        _cache = cache;

        // Global Cache Settings
        _cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
    }

    [Time("description = {description}")]
    [HttpGet("FindByDescription")]
    public async Task<IActionResult> FindByDescriptionAsync(string description)
    {
        var response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();
        try
        {
            // Is Valid Cache?
            if (_cache.TryGetValue("Categories", out IEnumerable<CategoriaDTO>? listaCategorias))
            {
                _logger.Info($"Read from cache {description}");
                response.Success = true;
                response.Data = listaCategorias!.Where(p => p.NombreCategoria.ToLower().Contains(description.ToLower())).ToList();
            }
            else
            {
                response = await _categoriaService.FindByDescriptionAsync(description);
            }

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    [Time]
    [HttpGet("List")]
    public async Task<IActionResult> ListAsync()
    {

        var response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();
        try
        {
            //Automatly Cache is clean once time is running out.
            if (_cache.TryGetValue("Categories", out IEnumerable<CategoriaDTO>? listaCategorias))
            {
                _logger.Info($"Read cache");
                response.Success = true;
                response.Data = listaCategorias!.ToList();
            }
            else
            {
                _logger.Info($"Cache created. Fetching from database.");
                // Getting from Database
                response = await _categoriaService.ListAsync();
                // Create the cache 
                _cache.Set("Categories", response.Data!.AsEnumerable(), _cacheEntryOptions);
            }

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    [Time("Id = {id}")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, CategoriaDTO request)
    {
        try
        {
            var response = await _categoriaService.UpdateAsync(id, request);

            if (response.Success)
            {

                // Is Valid Cache?
                if (_cache.TryGetValue("Categories", out List<CategoriaDTO>? listaCategorias))
                {
                    listaCategorias!.RemoveAll(f => f.IdCategoria == id);
                    listaCategorias.Add(request);
                    // Recreate cache
                    _cache.Set("Categories", listaCategorias.AsEnumerable(), _cacheEntryOptions);
                    _logger.Info($"Update cache Element Id - {id}");
                }
            }

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    [Time("Id = {id}")]
    [HttpGet("FindById")]
    public async Task<IActionResult> FindByIdAsync(int id)
    {
        CategoriaDTO categoria = null!;
        BaseResponseGeneric<ICollection<CategoriaDTO>> response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();
        try
        {
            // Is Valid Cache?
            if (_cache.TryGetValue("Categories", out IEnumerable<CategoriaDTO>? listaCategorias))
            {
                _logger.Info($"Read cache. Id = {id} ");
                categoria = listaCategorias!.Where(p => p.IdCategoria == id).FirstOrDefault()!;
                response.Success = true;
                var list = new List<CategoriaDTO>();
                list!.Add(categoria!);
                response.Data = list;
            }

            if (categoria is null)
            {
                response = await _categoriaService.FindByIdAsync(id);
            }

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    [Time]
    [HttpPost]
    public async Task<IActionResult> Post(CategoriaDTO request)
    {
        try
        {
            var response = await _categoriaService.AddAsync(request);

            if (response.Success)
            {
                // Add into Cache
                if (_cache.TryGetValue("Categories", out List<CategoriaDTO>? listaCategorias))
                {                    
                    listaCategorias!.Add(request);
                    _logger.Info($"Read cache and add new Object Id= {request.IdCategoria} ");
                    _cache.Set("Categories", listaCategorias.AsEnumerable(), _cacheEntryOptions);
                }
            }

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }


    [Time("Id = {id}")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var response = await _categoriaService.DeleteAsync(id);

            if (response.Success)
            {
                // Add into Cache
                if (_cache.TryGetValue("Categories", out List<CategoriaDTO>? listaCategorias))
                {
                    int index = listaCategorias!.FindIndex(f => f.IdCategoria == id)!;
                    listaCategorias[index].Estado = false;                     
                    _logger.Info($"Read cache and add update Id= {id} ");
                    _cache.Set("Categories", listaCategorias.AsEnumerable(), _cacheEntryOptions);
                }
            } 

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }
}
