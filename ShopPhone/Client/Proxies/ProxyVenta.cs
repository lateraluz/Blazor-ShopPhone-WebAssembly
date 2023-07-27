using ShopPhone.Shared;
using ShopPhone.Shared.Response;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopPhone.Client.Proxies;


public class ProxyVenta
{
    private readonly HttpClient _HttpClient;
    public ProxyVenta(HttpClient pHttpClient)
    {
        _HttpClient = pHttpClient;
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(FacturaDTO request)
    {

        string url = $"api/venta";
        var baseResponse = new BaseResponseGeneric<int>();
        string json = "";
        try
        {

            //ShopPhone.Shared.Response.BaseResponse<ProductoDTO> d = new ShopPhone.Shared.Response.BaseResponse<ProductoDTO>();
            var response = await _HttpClient.PostAsJsonAsync(url, request);

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
            var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<FacturaDTO>>>(url);
            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }


    public async void GetFactura() {
        try
        {
            string url = $"api/venta/myfactura";
            var response = await _HttpClient.GetAsync(url);
            //return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

}
