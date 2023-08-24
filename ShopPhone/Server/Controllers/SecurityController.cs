using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;
using System.Net;
using System.Reflection;

namespace ShopPhone.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("concurrency")]
public class SecurityController : ControllerBase
{
    private IUserService _userService;
    private ILog _logger;

    public SecurityController(IUserService pUserService, ILog logger)
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
                _logger.Warn(response.ErrorMessage);
                return Ok(response);
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                response.Success = false;
                response.ErrorMessage = "Password requerido";
                _logger.Warn(response.ErrorMessage);
                return Ok(response);
            }


            response = await _userService.LoginAsync(request);

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }

    }


}
