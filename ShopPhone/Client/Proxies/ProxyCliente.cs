using ShopPhone.Shared;
using ShopPhone.Shared.Response;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ShopPhone.Client.Proxies;


public class ProxyCliente
{
    private readonly HttpClient _httpClient;

    public ProxyCliente(HttpClient httpClient)
    {
        _httpClient =httpClient;
    }

    public async Task<BaseResponse> UpdateAsync(int id, ClienteDTO request)
    {
        string url = $"api/cliente/{id}";
        BaseResponse baseResponse = new();
        string json = "";

        try
        {
            var response = await _httpClient.PutAsJsonAsync(url, request);

            json = response.Content.ReadAsStringAsync().Result;

            baseResponse = JsonSerializer.Deserialize<BaseResponse>(json!, 
                                                  new JsonSerializerOptions{PropertyNameCaseInsensitive = true}
                                                  ) ??  throw new InvalidOperationException();

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
            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ClienteDTO>>>(url);
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
            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ClienteDTO>>>(url);

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
            var response = await _httpClient.PostAsJsonAsync(url, request);

            json = response.Content.ReadAsStringAsync().Result;

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

        string url = $"api/cliente/delete?id={id}";

        try
        {
            var response = await _httpClient.DeleteAsync(url);

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

    public async Task<BaseResponseGeneric<ICollection<ClienteDTO>>> ListAsync()
    {
        try
        {
            string url = $"api/Cliente/List";
            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ClienteDTO>>>(url);

            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

}

