using ShopPhone.Shared;
using ShopPhone.Shared.Response;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ShopPhone.Client.Proxies;


public class ProxyCliente
{
    private readonly HttpClient _HttpClient;

    public ProxyCliente(HttpClient pHttpClient)
    {
        _HttpClient = pHttpClient;
    }

    public async Task<BaseResponse> UpdateAsync(int id, ClienteDTO request)
    {
        string url = $"api/cliente/{id}";
        BaseResponse baseResponse = new();
        string json = "";

        try
        {
            var response = await _HttpClient.PutAsJsonAsync(url, request);

            json = response.Content.ReadAsStringAsync().Result;

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            baseResponse = JsonSerializer.Deserialize<BaseResponse>(json!, options) ??
                                            throw new InvalidOperationException();

            return baseResponse!;

        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<ClienteDTO>>> FindByIdAsync(int id)
    {
        try
        {
            string url = $"api/Cliente/FindById?id={id}";
            var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ClienteDTO>>>(url);
            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }


    public async Task<BaseResponseGeneric<ICollection<ClienteDTO>>> FindByDescriptionAsync(string description)
    {
        try
        {
            string url = $"api/Cliente/FindByDescription?description={description}";
            var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ClienteDTO>>>(url);

            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task<BaseResponse> AddAsync(ClienteDTO request)
    {

        string url = $"api/cliente";
        BaseResponse baseResponse = new();
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

            baseResponse = JsonSerializer.Deserialize<BaseResponse>(json!, options) ??
                                            throw new InvalidOperationException();

            return baseResponse!;

        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task DeleteAsync(int id)
    {

        string url = $"api/producto/cliente?id={id}";

        try
        {
            var response = await _HttpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }
}

