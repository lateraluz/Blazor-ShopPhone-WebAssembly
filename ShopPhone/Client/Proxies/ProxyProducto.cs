using ShopPhone.Shared.Response;
using System.Net.Http.Json;

namespace ShopPhone.Client.Proxies;

public class ProxyProducto
{
    private readonly HttpClient _HttpClient;

    public ProxyProducto(HttpClient pHttpClient)
    {
        _HttpClient = pHttpClient;
    }

    public async Task UpdateAsync(int id, ProductoDTO request)
    {
        string url = $"api/producto/{id}";
        var response = await _HttpClient.PutAsJsonAsync(url, request);

        if (response.IsSuccessStatusCode)
        {
            return;
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

            return response;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task AddAsync(ProductoDTO request)
    {

        string url = $"api/producto";

        try
        {
            var response = await _HttpClient.PostAsJsonAsync(url, request);

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
}
