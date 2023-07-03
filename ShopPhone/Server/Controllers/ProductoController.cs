using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Services.Implementations;
using ShopPhone.Shared.Response;
using ShopPhone.Shared.Util;
using System.Reflection;

namespace ShopPhone.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                string msg = UtilLog.Error(ex, MethodBase.GetCurrentMethod()!);
                _Logger.Error(msg, ex);
                throw;
            }
        }


        [HttpPut("{id:int}")]

        public async Task<IActionResult> Put(int id, ProductoDTO request)
        {
            await _ProductoService.UpdateAsync(id, request);

            return Ok();
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
                string msg = UtilLog.Error(ex, MethodBase.GetCurrentMethod()!);
                _Logger.Error(msg, ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductoDTO request)
        {
            try
            {
                var response = await _ProductoService.AddAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string msg = UtilLog.Error(ex, MethodBase.GetCurrentMethod()!);
                _Logger.Error(msg, ex);
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
                string msg = UtilLog.Error(ex, MethodBase.GetCurrentMethod()!);
                _Logger.Error(msg, ex);
                throw;
            }
        }


    }
}
