using Blazored.SessionStorage;
using ShopPhone.Shared;
using ShopPhone.Shared.Response;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopPhone.Client.Proxies;


public class ProxyVenta : Proxy
{
    private readonly HttpClient _httpClient;
    private const string _URL = $"/api/Security/refresh";
    private const string _IDSTORAGE = "token";


    public ProxyVenta(HttpClient httpClient, ISessionStorageService sessionStorage) :
        base(httpClient, sessionStorage, _URL!, _IDSTORAGE!)
    {
        _httpClient = httpClient;
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(FacturaDTO request)
    {

        string url = $"api/venta";
        var baseResponse = new BaseResponseGeneric<int>();
        string json = "";
        try
        {
            await RefreshToken();

            //ShopPhone.Shared.Response.BaseResponse<ProductoDTO> d = new ShopPhone.Shared.Response.BaseResponse<ProductoDTO>();
            var response = await _httpClient.PostAsJsonAsync(url, request);

            json = response.Content.ReadAsStringAsync().Result;

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            baseResponse = JsonSerializer.Deserialize<BaseResponseGeneric<int>>(json!, options) ??
                                            throw new InvalidOperationException();

            return baseResponse!;

        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<FacturaDTO>>> ListAsync()
    {
        try
        {
            string url = $"api/venta/List";

            await RefreshToken();

            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<FacturaDTO>>>(url);
            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }


}
