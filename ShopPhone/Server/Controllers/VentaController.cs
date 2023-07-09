using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Services.Implementations;
using ShopPhone.Shared.Response;
using System.Reflection;

namespace ShopPhone.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     
    public class VentaController : ControllerBase
    {
        private IVentaService _VentaService;
        private ILog _Logger;

        public VentaController(IVentaService service, ILog logger)
        {
            _VentaService = service;
            _Logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(FacturaDTO request)
        {
            try
            {
                var response = await _VentaService.AddAsync(request);

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
                var response = await _VentaService.ListAsync();

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

    }
}
