using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Services.Implementations;
using log4net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ShopPhone.Shared.Response;
using ShopPhone.DataAccess;
using ShopPhone.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopPhone.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class CategoriaController : ControllerBase
    {
        private IMemoryCache _cache;
        private ICategoriaService _categoriaService;
        private ILog _logger;
        public CategoriaController(ICategoriaService categoriaService, ILog logger, IMemoryCache cache)
        {
            _categoriaService = categoriaService;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("FindByDescription")]
        public async Task<IActionResult> FindByDescriptionAsync(string description)
        {
            try
            {
                var response = await _categoriaService.FindByDescriptionAsync(description);

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }
         
      
        [HttpGet("List")]
        public async Task<IActionResult> ListAsync()
        {

            BaseResponseGeneric<ICollection<CategoriaDTO>> response = new BaseResponseGeneric<ICollection<CategoriaDTO>>();
            try
            {
                //Automatly Cache is clean once time is running out.
                if (_cache.TryGetValue("ListCategory", out IEnumerable<CategoriaDTO> listaCategorias))
                {
                    _logger.Info("Read cache Lista Categorias");
                    response.Success = true;
                    response.Data = listaCategorias!.ToList();
                    
                }
                else
                {
                    _logger.Info("Cache created Lista Categorias. Fetching from database.");
                    // Getting from Database
                    response = await _categoriaService.ListAsync();
                    // Setting cache 
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);
                    _cache.Set("ListCategory", response.Data!.AsEnumerable(), cacheEntryOptions);
                }
  
                 return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, CategoriaDTO request)
        {
            try
            {                
                var response = await _categoriaService.UpdateAsync(id, request);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpGet("FindById")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            try
            {
                var response = await _categoriaService.FindByIdAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoriaDTO request)
        {
            try
            {
                var response = await _categoriaService.AddAsync(request);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }



        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _categoriaService.DeleteAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }
    }
}
