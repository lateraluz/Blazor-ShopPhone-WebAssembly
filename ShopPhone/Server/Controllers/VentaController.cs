using iTextSharp.text.pdf;
using iTextSharp.text;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Services.Implementations;
using ShopPhone.Shared.Response;
using System.Reflection;
using System.Reflection.Metadata;
using static iTextSharp.text.pdf.AcroFields;
using Microsoft.AspNetCore.Authorization;
using ShopPhone.Services.Interfaces;
using Microsoft.AspNetCore.RateLimiting;

namespace ShopPhone.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("concurrency")]
public class VentaController : ControllerBase
{
    private IVentaService _ventaService;
    private ILog _logger;

    public VentaController(IVentaService service, ILog logger)
    {
        _ventaService = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post(FacturaDTO request)
    {
        try
        {
            var response = await _ventaService.AddAsync(request);

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
            var response = await _ventaService.ListAsync();

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
            var response = await _ventaService.FindByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }    
}


