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

        private IClienteService _clienteService;
        private ILog _logger;
        public ClienteController(IClienteService service, ILog logger)
        {
            _clienteService = service;
            _logger = logger;
        }

        [HttpGet("FindByDescription")]
        public async Task<IActionResult> FindByDescriptionAsync(string description)
        {
            try
            {
                var response = await _clienteService.FindByDescriptionAsync(description);

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
            try
            {
                var response = await _clienteService.ListAsync();

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }


        [HttpPut("{id:int}")]

        public async Task<IActionResult> Put(int id, ClienteDTO request)
        {
            try
            {
                var response = await _clienteService.UpdateAsync(id, request);
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
                var response = await _clienteService.FindByIdAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClienteDTO request)
        {
            try
            {
                var response = await _clienteService.AddAsync(request);
                return Ok(response);
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
                var response = await _clienteService.DeleteAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }
    }
}
