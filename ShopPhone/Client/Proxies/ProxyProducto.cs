using Blazored.SessionStorage;
using ShopPhone.Shared;
using ShopPhone.Shared.Response;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopPhone.Client.Proxies;

public class ProxyProducto : Proxy
{
    private readonly HttpClient _httpClient;
    private const string _URL = $"/api/Security/refresh";
    private const string _IDSTORAGE = "token";

    public ProxyProducto(HttpClient httpClient, ISessionStorageService sessionStorage) :
        base(httpClient, sessionStorage, _URL!, _IDSTORAGE!)
    {
        _httpClient = httpClient;
    }


    public async Task<BaseResponse> UpdateAsync(int id, ProductoDTO request)
    {
        string url = $"api/producto/{id}";
        BaseResponse baseResponse = new() { Success = false };

        string json = "";

        try
        {

            await RefreshToken();

            var response = await _httpClient.PutAsJsonAsync(url, request);

            json = response.Content.ReadAsStringAsync().Result;

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (response.IsSuccessStatusCode)
            {
                baseResponse = JsonSerializer.Deserialize<BaseResponse>(json!, options) ??
                                                      throw new InvalidOperationException();
            }
            else
            {
                if (json.Contains("type") && json.Contains("status") && json.Contains("traceId"))
                {
                    baseResponse.ErrorMessage = json;
                    baseResponse.Success = false;
                }
                else
                {
                    baseResponse.ErrorMessage = "Error no se logró realizar la transacción " + json;
                    baseResponse.Success = false;
                }
            }
            return baseResponse!;
        }
        catch (Exception e)
        {
            Console.WriteLine(json);
            Exception ex = e;
            throw;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<ProductoDTO>>> FindByIdAsync(int id)
    {
        try
        {
            await RefreshToken();

            string url = $"api/producto/FindById?id={id}";
            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ProductoDTO>>>(url);
            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }


    public async Task<BaseResponseGeneric<ICollection<ProductoDTO>>> FindByDescriptionAsync(string description)
    {
        try
        {
            string url = $"api/producto/FindByDescription?description={description}";

            await RefreshToken();

            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ProductoDTO>>>(url);

            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task<BaseResponse> AddAsync(ProductoDTO request)
    {

        string url = $"api/producto";
        BaseResponse baseResponse = new();
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


            if (response.IsSuccessStatusCode)
            {
                baseResponse = JsonSerializer.Deserialize<BaseResponse>(json!, options) ??
                                                      throw new InvalidOperationException();
            }
            else
            {
                if (json.Contains("type") && json.Contains("status") && json.Contains("traceId"))
                {
                    baseResponse.ErrorMessage = json;
                    baseResponse.Success = false;
                }
                else
                {
                    baseResponse.ErrorMessage = "Error no se logró realizar la transacción " + json;
                    baseResponse.Success = false;
                }
            }

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

        string url = $"api/producto/delete?id={id}";

        try
        {
            await RefreshToken();

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

    public async Task<BaseResponseGeneric<ICollection<ProductoDTO>>> ListAsync()
    {
        try
        {
            string url = $"api/producto/List";

            await RefreshToken();

            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ProductoDTO>>>(url);
            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }
}
