using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;
using System.Net.Http.Json;
using System.Text.Json;

namespace ShopPhone.Client.Proxies;

public class ProxyUser
{
    private readonly HttpClient _HttpClient;

    public ProxyUser(HttpClient pHttpClient)
    {
        _HttpClient = pHttpClient;
    }


    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO? request)
    {

        string url = $"/api/Security/login";
       
        try
        {            
            var response = await _HttpClient.PostAsJsonAsync(url, request);

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            
            return loginResponse!;

        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task<LoginResponseDTO> LoginAsync2(LoginRequestDTO? request)
    {
        string url = $"api/login";

        var response = await _HttpClient.PostAsJsonAsync(url, request);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
        Console.WriteLine(loginResponse);
        if (loginResponse!.Success)
            return loginResponse;

        throw new InvalidOperationException(loginResponse.ErrorMessage);
    }

}