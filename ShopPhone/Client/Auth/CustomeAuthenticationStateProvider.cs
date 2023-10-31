using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using ShopPhone.Client.Auth;
using ShopPhone.Shared.Response;

namespace ShopPhone.Client.Auth;

public class CustomeAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _Anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    private readonly ISessionStorageService _SessionStorageService;
    private readonly HttpClient _HttpClient;

    public CustomeAuthenticationStateProvider(ISessionStorageService sessionStorageService, HttpClient httpClient)
    {
        _SessionStorageService = sessionStorageService;
        _HttpClient = httpClient;
    }

    public async Task Authenticate(LoginResponseDTO? response)
    {
        ClaimsPrincipal claimsPrincipal;

        if (response is not null)
        {
            // Recuperamos los claims desde el token recibido.
            var token = ParseToken(response);

            // Establecemos al objeto HttpClient el token en el header
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);

            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(token.Claims.ToList(), "JWT"));
            // Save session
            await _SessionStorageService.SaveStorage("token", response.Token);
            await _SessionStorageService.SaveStorage("rols", response.Roles[0]);
            await _SessionStorageService.SaveStorage("id", response.Identificacion.ToString());
            await _SessionStorageService.SaveStorage("names", response.FullName);
        }
        else
        {
            claimsPrincipal = _Anonymous;
            await _SessionStorageService.RemoveItemAsync("token");
            await _SessionStorageService.RemoveItemAsync("rols");
            await _SessionStorageService.RemoveItemAsync("id");
            await _SessionStorageService.RemoveItemAsync("names");
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sesionUsuario = await _SessionStorageService.GetStorage<LoginResponseDTO>("session");

        if (sesionUsuario is null)
            return await Task.FromResult(new AuthenticationState(_Anonymous));

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(ParseToken(sesionUsuario).Claims, "JWT"));

        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }
     

    public async Task Logout()
    {
        ClaimsPrincipal claimsPrincipal;
        claimsPrincipal = _Anonymous;
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
