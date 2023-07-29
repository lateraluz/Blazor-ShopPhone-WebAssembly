using log4net;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopPhone.Shared.Entities;
using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Implementations;

public class UserService : IUserService
{
   
    enum ELogs { Events = 0, Trace = 1 }

    private readonly IOptions<AppConfig> _Options;
   

    public UserService(IOptions<AppConfig> options)
    {
        _Options = options;

    }

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
    {
        var response = new LoginResponseDTO();
        string name = "Anthony Morera";
        string email = "anthony.morera@gmail.com";
        DateTime expiredDate;
        List<string> opciones = new List<string>();
        try
        {        

            // Validate User
            if (!(request.UserName == "admin" && request.Password == "123456*"))
            {               
                throw new SecurityException("Usuario no existe");
            }

            // Get Menu Options
            opciones.Add("Opcion1");
            opciones.Add("Opcion2");

            expiredDate = DateTime.Now.AddHours(2);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Sid, request.UserName));
            claims.Add(new Claim(ClaimTypes.WindowsAccountName, request.UserName));
            claims.Add(new Claim(ClaimTypes.Name, name));
            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim(ClaimTypes.Expiration, expiredDate.ToString("dd-MM-yyyy HH:mm:ss")));

            claims.AddRange(opciones.Select(c => new Claim(ClaimTypes.Role, c)));
            /*
            response.MenuOptions = new List<string>();
            response.MenuOptions.AddRange(opciones);
            */
            // Creacion del JWT
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Options.Value.Jwt.SecretKey));

            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(_Options.Value.Jwt.Issuer,
                _Options.Value.Jwt.Audience,
                claims,
                DateTime.Now,
                expiredDate);

            var token = new JwtSecurityToken(header, payload);
            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            response.FullName = $"{name}";
            response.Success = true;
            response.Roles = new List<string>();
            response.Roles.Add("Opcion1");
        }
        catch (SecurityException ex)
        {
            response.ErrorMessage = ex.Message;
           
        }
        catch (Exception e)
        {
            response.ErrorMessage = "Error al momento de hacer la autenticacion";
           
        }


        return await Task.FromResult(response);
    }

     
}