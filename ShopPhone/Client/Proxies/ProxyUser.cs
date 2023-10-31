using ApexCharts;
using AutoMapper.Internal;
using Blazored.SessionStorage;
using Microsoft.Extensions.Options;
using ShopPhone.Client.Auth;
using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Util = ShopPhone.Shared.Util.Util;
namespace ShopPhone.Client.Proxies;

public class ProxyUser
{
    private readonly HttpClient _httpClient;
    private ISessionStorageService _sessionStorage;

    public ProxyUser(HttpClient pHttpClient, ISessionStorageService sessionStorage)
    {
        _httpClient = pHttpClient;
        _sessionStorage = sessionStorage;
    }

    


    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO? request)
    {
        string url = $"/api/Security/login";
        HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
        LoginResponseDTO response = new LoginResponseDTO();
        try
        {
            //request.UserName = null;
            httpResponseMessage = await _httpClient.PostAsJsonAsync(url, request);

            /*
             // Validate posible returns 
             1- Json   {"type":"https://tools.ietf.org/html/rfc7231#section-6.5.1","title":"One or more validation errors occurred.","status":400,"traceId":"00-92b6d12047f71312b1ab71666b0c2aa3-82dc2310d8bb7f37-00","errors":{"Password":["The Password field is required."],"UserName":["The UserName field is required."]}}
             2- return BadRequest("Dummy Error ");
             3- return (DTO) Object Success true
             4- return (DTO) Object Success false
            */
            //if (!(httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK))
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                response = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseDTO>() ?? throw new Exception("Error de conexión en Login");

                if (!response!.Success)
                {
                    response.ErrorMessage = response.ErrorMessage + " [{httpResponseMessage.StatusCode}]";
                    return response;
                }

                string json = httpResponseMessage.Content.ReadAsStringAsync().Result;
                // It is a Json?
                if (Util.IsValidJson(json))
                {
                    response.Success = false;
                    response.ErrorMessage = $"{httpResponseMessage.ReasonPhrase} - {Util.GetStandarErrorMessages(json)} [{httpResponseMessage.StatusCode}]";
                }
                else
                {
                    // It is a posible return like BadRequest("Dummy Error ")
                    response!.Success = false;
                    response.ErrorMessage = httpResponseMessage!.Content.ReadAsStringAsync().Result;
                }
            }
            else
            {
                response = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseDTO>() ?? throw new Exception("Error de conexión en Login");
            }

            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }
    }




    public async Task RefreshToken()
    {
        string url = $"/api/Security/refresh";
        HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

        try
        {

            var (isTokenOutofTime, token) = await GetTokenStorage("token");

            if (!isTokenOutofTime && string.IsNullOrEmpty(token))
            {
                return;
            }
            RefreshTokenDTO refreshToken = new RefreshTokenDTO()
            {
                Token = token
            };

            //request.UserName = null;
            httpResponseMessage = await _httpClient.PostAsJsonAsync(url, refreshToken);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RefreshTokenDTO>() ?? throw new Exception("Error de conexión en Login");

                if (response.Success)
                {                
                    await _sessionStorage.SaveStorage<string>("token", response.Token);
                    // Testing
                    await _sessionStorage.SaveStorage<string>("OLDtoken", token);
                    return;
                }
                else
                {
                    throw new Exception("Error Refresh Token!");
                }

            }
            else
            {
                throw new Exception("Error Refresh Token!");
            }

        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }
    }


    private async Task<(bool, string)> GetTokenStorage(string tokenId)
    {

        DateTimeOffset dateTimeOffset = new DateTimeOffset();
        var token = await _sessionStorage.GetItemAsync<string>(tokenId);

        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Error en Seguridad!");
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var unixDateTime = jwtToken.Claims.ToList().Find(p => p.Type == "exp")!.Value!;

        if (string.IsNullOrEmpty(unixDateTime))
        {
            throw new Exception("Error en Seguridad!");
        }

        dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixDateTime));
        /* Test Refresh 
        if (dateTimeOffset.DateTime > DateTime.Now && dateTimeOffset.DateTime.Subtract(DateTime.Now).TotalMinutes > 5)
        {
            return (false, "");
        }
        */
        return (true, token);
        
    }



}