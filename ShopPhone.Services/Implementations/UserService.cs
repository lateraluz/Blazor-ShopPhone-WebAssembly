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
using System.Reflection;
using System.Security.Cryptography;
using ShopPhone.Shared.Util;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Services.Interfaces;

namespace ShopPhone.Services.Implementations;

public class UserService : IUserService
{

    private readonly IOptions<AppConfig> _options;
    private IUserRepository _userRepository;
    private ILog _logger;

    public UserService(IOptions<AppConfig> options, IUserRepository repository, ILog logger)
    {
        _options = options;
        _userRepository = repository;
        _logger = logger;

    }

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
    {
        var response = new LoginResponseDTO();
        string passwordCrypted = "";
        DateTime expiredDate;
        List<string> roles = new List<string>();
        try
        {
            var user = await _userRepository.FindAsync(request.UserName.Trim());

            // Encriptar para comparar
            passwordCrypted = Cryptography.EncryptAes(request.Password.Trim());


            if (user == null)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Usuario no existe {request.UserName}");
                throw new SecurityException("Usuario no existe");
            }

            if (!user.Estado)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Usuario está deshabilitado {request.UserName}");
                throw new SecurityException("Usuario deshabilitado, contacte al administrador");
            }

            if (user.Contrasena.Trim().Equals(passwordCrypted.Trim()) == false)
            {
                _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Usuario con contraseña no valida: {request.UserName}");
                throw new SecurityException("Verfique su usuario / contraseña");
            }

            roles.Add(user.IdRol.Trim());

            
            expiredDate = DateTime.Now.AddHours(2);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Sid, request.UserName));
            claims.Add(new Claim(ClaimTypes.WindowsAccountName, request.UserName));
            claims.Add(new Claim(ClaimTypes.Name, user.Nombre.Trim() + " " + user.Apellidos.Trim()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email.Trim()));
            claims.Add(new Claim(ClaimTypes.Expiration, expiredDate.ToString("dd-MM-yyyy HH:mm:ss")));

            claims.AddRange(roles.Select(c => new Claim(ClaimTypes.Role, c)));
           
            // Creacion del JWT
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Jwt.SecretKey));

            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(_options.Value.Jwt.Issuer,
                _options.Value.Jwt.Audience,
                claims,
                DateTime.Now,
                expiredDate);

            var token = new JwtSecurityToken(header, payload);
            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            response.FullName = $"{user.Nombre.Trim()} {user.Apellidos.Trim()}";
            response.Success = true;
            response.Roles = new List<string>();
            response.Roles.Add(user.IdRol);

            return response;
        }
        catch (SecurityException ex1)
        {
            response.ErrorMessage = ex1.Message;
            _logger.Error($"{response.ErrorMessage} en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex1);
            return response;

        }
        catch (Exception ex2)
        {
            response.ErrorMessage = "Error al momento de hacer la autenticacion";
            _logger.Error($"{response.ErrorMessage} en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex2);
            return response; 
        }

    }
}