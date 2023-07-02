
using ShopPhone.Shared.Response;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;


namespace ShopPhone.Client.Proxies;

public class ProxyCategoria
{

    private readonly HttpClient _HttpClient;


    public ProxyCategoria(HttpClient pHttpClient)
    {
        _HttpClient = pHttpClient;
    }

    public async Task UpdateAsync(int id, CategoriaDTO request)
    {
        string url = $"api/categoria/{id}";
        var response = await _HttpClient.PutAsJsonAsync(url, request);

        if (response.IsSuccessStatusCode)
        {
            return;
        }         
    }

    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByIdAsync(int id)
    {
        try
        {
            string url = $"api/categoria/FindById?id={id}";
            var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<CategoriaDTO>>>(url);
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
            var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<CategoriaDTO>>>(url);

            return response!;
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }

    }

    public async Task AddAsync(CategoriaDTO request)
    {

        string url = $"api/categoria";

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

        string url = $"api/categoria/delete?id={id}";

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
