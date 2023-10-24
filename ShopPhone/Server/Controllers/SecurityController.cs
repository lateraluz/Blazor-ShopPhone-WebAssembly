using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShopPhone.Server.Extensions;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace ShopPhone.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("concurrency")]
public class SecurityController : ControllerBase
{
    private IUserService _userService;
    private ILogger<SecurityController> _logger;
    private IValidator<LoginRequestDTO> _validator;

    public SecurityController(IUserService pUserService, ILogger<SecurityController> logger, IValidator<LoginRequestDTO> validator)
    {
        _userService = pUserService;
        _logger = logger;
        _validator = validator;
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

            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.ErrorMessage = validationResult.ToListErrorsString();
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


    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenDTO request)
    {
         
        try
        {
            if (request is null)
            {
                return BadRequest(new RefreshTokenDTO
                {
                    Success = false,
                    ErrorMessage = "Invalid client request"
                });
            }

            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(new RefreshTokenDTO
                {
                    Success = false,
                    ErrorMessage = "Invalid client request"
                });
            }

            var response = await _userService.RefreshAsync(request);

            return Ok(response);

        }
        catch (Exception ex)
        {
            Exception err = ex;
            return BadRequest(new RefreshTokenDTO { Success = false, ErrorMessage = "Security error" });

        }
    }

}



