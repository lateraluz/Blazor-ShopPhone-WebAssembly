using log4net;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Request;

namespace ShopPhone.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecurityController : ControllerBase
{
    private IUserService _UserService;

    public SecurityController(IUserService pUserService)
    {
        _UserService = pUserService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequestDTO request)
    {


        if (request == null)         
            return BadRequest("Error sin recibir parámetros");
         

        if (string.IsNullOrEmpty(request.UserName))        
            return BadRequest("Login es requerido");
        

        if (string.IsNullOrEmpty(request.Password))
            return BadRequest("Password es requerido");
        
        var response = await _UserService.LoginAsync(request);

        return response.Success ? Ok(response) : NotFound(response);
    }


}
