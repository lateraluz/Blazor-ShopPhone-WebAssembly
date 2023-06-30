
using ShopPhone.Shared.Response;
using System.Net.Http.Json;

namespace ShopPhone.Client.Proxies;

public class ProxyCategoria
{

    private readonly HttpClient _HttpClient;

    public ProxyCategoria(HttpClient pHttpClient)
    {
        _HttpClient = pHttpClient;
    }

    public async Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByDescriptionAsync(string description)
    {
        string url = $"api/categoria/FindByDescription?description={description}";
        var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<CategoriaDTO>>>(url);

        return response!;
    }

    /*
    public async Task<BaseResponseGeneric<ICollection<ExtranjeroSolicitanteDTO>>> FindExtranjeroByIdAsync(string Id)
    {
        string url = $"api/extranjero/FindExtranjeroById?id={Id}";
        var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<ExtranjeroSolicitanteDTO>>>(url);

        return response!;
    }

    public async Task<BaseResponseGeneric<ICollection<DetalleCobro>>> GetPaymentById(string Id)
    {
        string url = $"api/extranjero/GetPaymentById?id={Id}";
        var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<DetalleCobro>>>(url);

        return response!;
    }
    public async Task<BaseResponseGeneric<ICollection<DetalleCobro>>> CalculateCharge(string TipoCategoria, string FechaInicial, string FechaFinal)
    {
        string url = $"api/extranjero/CalculateCharge?TipoCategoria={TipoCategoria}&FechaInicial={FechaInicial}&FechaFinal={FechaFinal}";
        var response = await _HttpClient.GetFromJsonAsync<BaseResponseGeneric<ICollection<DetalleCobro>>>(url);

        return response!; 
    }
    */
}
