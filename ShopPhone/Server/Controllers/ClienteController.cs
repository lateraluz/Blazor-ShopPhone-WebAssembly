using Serilog;
using MethodTimer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;
using System.Reflection;
using ILogger = Serilog.ILogger;
using FluentValidation;
using ShopPhone.Server.Extensions;

namespace ShopPhone.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("concurrency")]
public class ClienteController : ControllerBase
{

    private IClienteService _clienteService;
    private ILogger<ClienteController> _logger;
    private IValidator<ClienteDTO> _validator;
    public ClienteController(IClienteService service,
                             ILogger<ClienteController> logger,
                              IValidator<ClienteDTO> validator)
    {
        _clienteService = service;
        _logger = logger;
        _validator = validator;
    }

    [Time("description = {description}")]
    [HttpGet("FindByDescription")]
    public async Task<IActionResult> FindByDescriptionAsync(string description)
    {
        try
        {
            if (string.IsNullOrEmpty(description))
            {

                var validResponse = new BaseResponse() { 
                
                    Success = false,
                    ErrorMessage = "La descripción es un dato requerido"
                };
                
                return Ok(validResponse);
            }


            var response = await _clienteService.FindByDescriptionAsync(description);

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    [Time]
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
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }


    [Time("id = {id}")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, ClienteDTO request)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                BaseResponseGeneric<int> validResponse = new();
                validResponse.Success = false;
                validResponse.ErrorMessage = validationResult.ToListErrorsString();
                return Ok(validResponse);
            }


            var response = await _clienteService.UpdateAsync(id, request);
            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    [Time("id = {id}")]
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
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(ClienteDTO request)
    {
        try
        {

            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                BaseResponseGeneric<int> validResponse = new();
                validResponse.Success = false;
                validResponse.ErrorMessage = validationResult.ToListErrorsString();
                return Ok(validResponse);
            }

            var response = await _clienteService.AddAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }


    [Time("id = {id}")]
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
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }
}
