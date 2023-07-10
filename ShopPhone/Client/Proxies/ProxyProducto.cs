using ShopPhone.Shared;
using ShopPhone.Shared.Response;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopPhone.Client.Proxies;

public class ProxyProducto
{
    private readonly HttpClient _HttpClient;

    public ProxyProducto(HttpClient pHttpClient)
    {
        _HttpClient = pHttpClient;
    }

    public async Task<BaseResponse> UpdateAsync(int id, ProductoDTO request)
    {
        string url = $"api/producto/{id}";
        BaseResponse baseResponse = new() { Success = false };

        string json = "";

        try
        {
            var response = await _HttpClient.PutAsJsonAsync(url, request);

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
                    var customError = JsonSerializer.Deserialize<CustomeError>(json!, options) ??
                                                  throw new InvalidOperationException();

                    baseResponse.ErrorMessage = customError.Title;
                    baseResponse.Success = false;
                }
                else
                {
                    baseResponse.ErrorMessage = "Error no se logró realizar la transacción";
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
            string url = $"api/producto/FindById?id={id}";
            var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ProductoDTO>>>(url);
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
            var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ProductoDTO>>>(url);

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

        string url = $"api/producto/delete?id={id}";

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

    public async Task<BaseResponseGeneric<ICollection<ProductoDTO>>> ListAsync()
    {
        try
        {
            string url = $"api/producto/List";
            var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ProductoDTO>>>(url);
            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }
}
