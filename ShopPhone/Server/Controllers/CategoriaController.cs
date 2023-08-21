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
        private IMemoryCache _Cache;
        private ICategoriaService _CategoriaService;
        private ILog _Logger;
        public CategoriaController(ICategoriaService categoriaService, ILog logger, IMemoryCache cache)
        {
            _CategoriaService = categoriaService;
            _Logger = logger;
            _Cache = cache;
        }

        [HttpGet("FindByDescription")]
        public async Task<IActionResult> FindByDescriptionAsync(string description)
        {
            try
            {
                var response = await _CategoriaService.FindByDescriptionAsync(description);

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
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
                if (_Cache.TryGetValue("ListCategory", out IEnumerable<CategoriaDTO> listaCategorias))
                {
                    _Logger.Info("Read cache Lista Categorias");
                    response.Success = true;
                    response.Data = listaCategorias!.ToList();
                    return Ok(response);
                }
                else
                {
                    _Logger.Info("Create cache Lista Categorias. Fetching from database.");
                    // Getting from Database
                    response = await _CategoriaService.ListAsync();
                    // Setting cache 
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(120))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);
                    _Cache.Set("ListCategory", response.Data!.AsEnumerable(), cacheEntryOptions);
                }
  
                 return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }


        [HttpPut("{id:int}")]

        public async Task<IActionResult> Put(int id, CategoriaDTO request)
        {
            try
            {
                
                var response = await _CategoriaService.UpdateAsync(id, request);
                return Ok(response);

            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpGet("FindById")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            try
            {
                var response = await _CategoriaService.FindByIdAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoriaDTO request)
        {
            try
            {
                var response = await _CategoriaService.AddAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }



        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _CategoriaService.DeleteAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }



    }
}
