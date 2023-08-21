using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;
using System.Reflection;

namespace ShopPhone.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("concurrency")]
    public class ClienteController : ControllerBase
    {

        private IClienteService _ClienteService;
        private ILog _Logger;
        public ClienteController(IClienteService service, ILog logger)
        {
            _ClienteService = service;
            _Logger = logger;
        }

        [HttpGet("FindByDescription")]
        public async Task<IActionResult> FindByDescriptionAsync(string description)
        {
            try
            {
                var response = await _ClienteService.FindByDescriptionAsync(description);

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
                var response = await _ClienteService.ListAsync();

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }


        [HttpPut("{id:int}")]

        public async Task<IActionResult> Put(int id, ClienteDTO request)
        {
            try
            {
                var response = await _ClienteService.UpdateAsync(id, request);
                return response.Success ? Ok(response) : NotFound(response);
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
                var response = await _ClienteService.FindByIdAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClienteDTO request)
        {
            try
            {
                var response = await _ClienteService.AddAsync(request);
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
                var response = await _ClienteService.DeleteAsync(id);
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
