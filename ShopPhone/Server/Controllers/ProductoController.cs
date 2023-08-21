using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;
using System.Net;
using System.Reflection;

namespace ShopPhone.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("concurrency")]
    public class ProductoController : ControllerBase
    {
        private IProductoService _ProductoService;
        private ILog _Logger;
        public ProductoController(IProductoService service, ILog logger)
        {
            _ProductoService = service;
            _Logger = logger;
        }

        [HttpGet("FindByDescription")]
        public async Task<IActionResult> FindByDescriptionAsync(string description)
        {
            try
            {
                var response = await _ProductoService.FindByDescriptionAsync(description);

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
            try
            {
                var response = await _ProductoService.ListAsync();

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }


        [HttpPut("{id:int}")]

        public async Task<IActionResult> Put(int id, ProductoDTO request)
        {
            try
            {
                var response = await _ProductoService.UpdateAsync(id, request);
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
                var response = await _ProductoService.FindByIdAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductoDTO request)
        {
            try
            {
                var response = await _ProductoService.AddAsync(request);

                return response.Success ? Ok(response) : NotFound(response);
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
                var response = await _ProductoService.DeleteAsync(id);
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
