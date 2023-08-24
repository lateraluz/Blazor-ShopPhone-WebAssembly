
using ShopPhone.Shared.Response;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text.Json;

namespace ShopPhone.Client.Proxies;

public class ProxyCategoria
{

    private readonly HttpClient _httpClient;

    public ProxyCategoria(HttpClient pHttpClient)
    {
        _httpClient = pHttpClient;
    }

    public async Task<BaseResponse> UpdateAsync(int id, CategoriaDTO request)
    {
        string url = $"api/categoria/{id}";
        string json = "";
        try
        {
            var response = await _httpClient.PutAsJsonAsync(url, request);
            json = response.Content.ReadAsStringAsync().Result;
            var baseResponse = JsonSerializer.Deserialize<BaseResponse>(json!,
                                                        new JsonSerializerOptions
                                                        { PropertyNameCaseInsensitive = true })
                                                         ??
                                                         throw new InvalidOperationException();

            return baseResponse!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }
    }

    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByIdAsync(int id)
    {
        string url = $"api/categoria/FindById?id={id}";
        try
        {
            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<CategoriaDTO>>>(url);
            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> ListAsync()
    {
        try
        {
            string url = $"api/categoria/List";
            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<CategoriaDTO>>>(url);
            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }


    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByDescriptionAsync(string description)
    {
        try
        {
            string url = $"api/categoria/FindByDescription?description={description}";
            var response = await _httpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<CategoriaDTO>>>(url);

            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task<BaseResponse> AddAsync(CategoriaDTO request)
    {
        string url = $"api/categoria";

        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, request);
            string json = response.Content.ReadAsStringAsync().Result;
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var baseResponse = JsonSerializer.Deserialize<BaseResponse>(json!, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true })
                                                                ??
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

        string url = $"api/categoria/delete?id={id}";

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


}
