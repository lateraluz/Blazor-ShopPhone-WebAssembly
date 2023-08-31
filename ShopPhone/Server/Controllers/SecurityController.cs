using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;
using System.Net;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace ShopPhone.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("concurrency")]
public class SecurityController : ControllerBase
{
    private IUserService _userService;
    private ILogger<SecurityController> _logger;

    public SecurityController(IUserService pUserService, ILogger<SecurityController> logger)
    {
        _userService = pUserService;
        _logger = logger;
    }


    [HttpPost("login")]
    [EnableRateLimiting("concurrency")]
    public async Task<IActionResult> LoginAsync(LoginRequestDTO request)
    {
        var response = new LoginResponseDTO();
        
        try
        {
            /*
            _logger.LogInformation("*LogInformation");
            _logger.LogCritical("*LogCritical");
            _logger.LogError("*LogError");
            _logger.LogWarning("*LogWarning");
            _logger.LogTrace("*LogTrace");
            _logger.LogDebug("*LogDebug");
            */

            // Tested responses 
            // return BadRequest("Dummy: BadRequest");
            // return Ok("Dummy: Everthing is Ok ");
            // return NotFound("Dumy: It doesn't exist!");
            // throw new Exception("My dummy exception!");
            if (request == null)
            {
                response.Success = false;
                response.ErrorMessage = "Error en parámetros";
                return Ok(response);
            }

            if (string.IsNullOrEmpty(request.UserName))
            {
                response.Success = false;
                response.ErrorMessage = "Usuario requerido";
                _logger.LogWarning(response.ErrorMessage);
                return Ok(response);
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                response.Success = false;
                response.ErrorMessage = "Password requerido";
                _logger.LogWarning(response.ErrorMessage);
                return Ok(response);
            }

            
            response = await _userService.LoginAsync(request);
            if (response.Success)
                _logger.LogInformation($"Logged {request.UserName}");
            else
                _logger.LogError($"Error Logged {request.UserName}");
            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }

    }


}
