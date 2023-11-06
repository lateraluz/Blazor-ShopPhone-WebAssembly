using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ShopPhone.Shared.Response;

namespace ShopPhone.Client.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    private readonly ISessionStorageService _sessionStorageService;
    private readonly HttpClient _httpClient;

    public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService, HttpClient httpClient)
    {
        _sessionStorageService = sessionStorageService;
        _httpClient = httpClient;
    }

    public async Task Authenticate(LoginResponseDTO? response)
    {
        ClaimsPrincipal claimsPrincipal;

        if (response is not null)
        {
            // Recuperamos los claims desde el token recibido.
            var token = ParseToken(response);

            // Establecemos al objeto HttpClient el token en el header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);

            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(token.Claims.ToList(), "JWT"));
            // Save session
            await _sessionStorageService.SetItemAsync<string>("token", response.Token);
            await _sessionStorageService.SetItemAsync<string>("rols", response.Roles[0]);
            await _sessionStorageService.SetItemAsync<string>("id", response.Identificacion.ToString());
            await _sessionStorageService.SetItemAsync<string>("names", response.FullName);
        }
        else
        {
            claimsPrincipal = _anonymous;
            await _sessionStorageService.RemoveItemAsync("token");
            await _sessionStorageService.RemoveItemAsync("rols");
            await _sessionStorageService.RemoveItemAsync("id");
            await _sessionStorageService.RemoveItemAsync("names");
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sesionUsuario = await _sessionStorageService.GetItemAsync<LoginResponseDTO>("session");

        if (sesionUsuario is null)
            return await Task.FromResult(new AuthenticationState(_anonymous));

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(ParseToken(sesionUsuario).Claims, "JWT"));

        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }
     

    public async Task Logout()
    {
        ClaimsPrincipal claimsPrincipal;
        claimsPrincipal = _anonymous;
        await Task.FromResult(claimsPrincipal);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }


    public static JwtSecurityToken ParseToken(LoginResponseDTO sesionUsuario)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(sesionUsuario.Token);
        return token;
    }
}
