using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopPhone.Shared.Entities;
using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security;
using System.Text;
using System.Reflection;
using ShopPhone.Shared.Util;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using ShopPhone.DataAccess;
using Azure;
using System.Data;

namespace ShopPhone.Services.Implementations;

public class UserService : IUserService
{

    private readonly IOptions<AppConfig> _options;
    private IUserRepository _userRepository;
    private ILogger<UserService> _logger;

    public UserService(IOptions<AppConfig> options, IUserRepository repository, ILogger<UserService> logger)
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

            _logger.LogInformation("Applying Cripto");
            if (user == null)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Usuario no existe {request.UserName}");
                throw new SecurityException("Usuario no existe");
            }

            if (!user.Estado)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Usuario está deshabilitado {request.UserName}");
                throw new SecurityException("Usuario deshabilitado, contacte al administrador");
            }

            if (user.Contrasena.Trim().Equals(passwordCrypted.Trim()) == false)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Usuario con contraseña no valida: {request.UserName}");
                throw new SecurityException("Verfique su usuario / contraseña");
            }

            roles.Add(user.IdRol.Trim());


            expiredDate = DateTime.Now.AddMinutes(_options.Value.Jwt.TTL);

            List<Claim> claims = new List<Claim>();
            claims!.Add(new Claim(ClaimTypes.Sid, request.UserName));
            claims!.Add(new Claim(ClaimTypes.WindowsAccountName, request.UserName));
            claims!.Add(new Claim(ClaimTypes.Name, user.Nombre.Trim() + " " + user.Apellidos.Trim()));
            claims!.Add(new Claim(ClaimTypes.Email, user.Email.Trim()));
            //claims!.Add(new Claim(ClaimTypes.Expiration, expiredDate.ToString("dd-MM-yyyy HH:mm:ss")));

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
            response!.Roles.Add(user.IdRol);

            return response;
        }
        catch (SecurityException ex1)
        {
            response.Success = false;
            response.ErrorMessage = ex1.Message;
            _logger.LogError($"{response.ErrorMessage} en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex1);
            return response;

        }
        catch (Exception ex2)
        {
            response.Success = false;
            response.ErrorMessage = "Error de Seguridad";
            _logger.LogError($"{response.ErrorMessage} en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex2);
            return response;
        }
    }
    
    public async Task<RefreshTokenDTO> RefreshAsync(RefreshTokenDTO request)
    {
        RefreshTokenDTO response = new RefreshTokenDTO();
        List<string> roles = new List<string>();
        try
        {

            var claimPrincipal = GetClaimPrincipalFromExpiredToken(request.Token);

            // Get all claims
            var claimSid = claimPrincipal.Claims.ToList().FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid");
            var claimWindowsAccount = claimPrincipal.Claims.ToList().FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname");
            var claimWindowsName = claimPrincipal.Claims.ToList().FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var claimEmailAddress = claimPrincipal.Claims.ToList().FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            var claimRole = claimPrincipal.Claims.ToList().FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            if (claimSid is null || claimWindowsAccount is null || claimWindowsName is null || claimEmailAddress is null || claimRole is null)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Claim || claimWindowsAccount || claimWindowsName ||  claimEmailAddress || claimRole (null)");
                return new RefreshTokenDTO()
                {
                    Success = false,
                    ErrorMessage = "Error de Seguridad"
                };
            }

            // Issuer
            var issuer = claimPrincipal!.Claims!.ToList()!.FirstOrDefault(x => x.Type == "iss")!.Value!;
            // Audience
            var audience = claimPrincipal!.Claims!.ToList()!.FirstOrDefault(x => x.Type == "aud")!.Value!;

            // validate 
            if (!issuer.Equals(_options.Value.Jwt.Issuer, StringComparison.InvariantCultureIgnoreCase)) {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Error Validation Issuer are different");
                return new RefreshTokenDTO()
                {
                    Success = false,
                    ErrorMessage = "Error de Seguridad"
                };
            }

            if (!audience.Equals(_options.Value.Jwt.Audience, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Error Validation Validation are different");
                return new RefreshTokenDTO()
                {
                    Success = false,
                    ErrorMessage = "Error de Seguridad"
                };

            }
             

            // Validate dateTime expiration
            var unixTime = claimPrincipal!.Claims!.ToList()!.FirstOrDefault(x => x.Type == "exp")!.Value!;

            if (string.IsNullOrEmpty(unixTime))
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} UnixTime viene vacío");
                return new RefreshTokenDTO()
                {
                    Success = false,
                    ErrorMessage = "Error de Seguridad"
                };
            }

            if (!long.TryParse(unixTime, out long result))
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} UnixTime no se puede convertir a Long");
                return new RefreshTokenDTO()
                {
                    Success = false,
                    ErrorMessage = "Error de Seguridad"
                };
            }

            // Convert UnixTime to DateTime
            var fechaExpiracion = DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTime));
            fechaExpiracion = fechaExpiracion.DateTime.AddHours(-6); // CRI -6 

            // Token out of time ?
            if (fechaExpiracion.DateTime > DateTime.Now && fechaExpiracion.DateTime.Subtract(DateTime.Now).TotalMinutes > 5)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Token wasnt refreshed cause isnt out of time");

                response.Success = true;
                response.Token = request.Token;
                return response;
            }

            
            //string username = principal.Identity.Name;
            var user = await _userRepository.FindAsync(claimSid.Value!);

            if (user == null)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} user doesnt exist!. Tamper security ");
                return new RefreshTokenDTO()
                {
                    Success = false,
                    ErrorMessage = "Error de Seguridad"
                };
            }

            roles.Add(user.IdRol.Trim());

            List<Claim> claims = new List<Claim>();
            claims!.Add(new Claim(ClaimTypes.Sid, claimSid.Value!));
            claims!.Add(new Claim(ClaimTypes.WindowsAccountName, claimWindowsAccount.Value));
            claims!.Add(new Claim(ClaimTypes.Name, claimWindowsName.Value));
            claims!.Add(new Claim(ClaimTypes.Email, claimEmailAddress.Value));
            

            claims.AddRange(roles.Select(c => new Claim(ClaimTypes.Role, c)));

            // Creacion del JWT
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Jwt.SecretKey));
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(credentials);
            var expiredDate = DateTime.Now.AddMinutes(_options.Value.Jwt.TTL);
            var payload = new JwtPayload(_options.Value.Jwt.Issuer,
                                        _options.Value.Jwt.Audience,
                                        claims,
                                        DateTime.Now,
                                        expiredDate);

            var token = new JwtSecurityToken(header, payload);
            response.Token = new JwtSecurityTokenHandler().WriteToken(token); 
            response.Success = true;

            _logger.LogInformation($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName} Token was updated! ");
            return response;

        }
        catch (SecurityException ex1)
        {
            response.Success = false;
            response.ErrorMessage = "Error de Seguridad";
            _logger.LogError($"{response.ErrorMessage} en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex1);
            return response;

        }
        catch (Exception ex2)
        {
            response.Success = false;
            response.ErrorMessage = "Error de Seguridad";
            _logger.LogError($"{response.ErrorMessage} en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex2);
            return response;
        }
    }
     
   

    public ClaimsPrincipal GetClaimPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Jwt.SecretKey)),
            ValidateLifetime = false
        };

      
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogError($"Token Invalido en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}");
            throw new SecurityTokenException("Error de Seguridad");
        }

        return principal;
    }

    
}