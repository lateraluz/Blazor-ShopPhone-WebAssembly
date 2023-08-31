using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using ShopPhone.Shared.Entities;
using System.Reflection;
using System.Net;
using System.Security.Policy;
using System;

namespace ShopPhone.Server.Health;

public class ImageHealthCheck : IHealthCheck
{
    private IOptions<AppConfig> _option;
    private ILogger<ImageHealthCheck> _logger;
    private HttpClient _httpClient  ;

    public ImageHealthCheck(IOptions<AppConfig> option, ILogger<ImageHealthCheck> logger , HttpClient httpClient)
    {
        _option = option;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        string url;
        bool exists = false;
        string contentType = "";
        ulong contentLength = 0UL;
        try
        {
            url = _option.Value.StorageConfiguration.PublicUrl;
         

            using var request = new HttpRequestMessage(HttpMethod.Head, url);

            using HttpResponseMessage response = await _httpClient.SendAsync(request);

            exists = response.StatusCode == HttpStatusCode.OK;

            if (response.Headers.TryGetValues(HttpResponseHeader.ContentType.ToString(), out IEnumerable<string>? contentTypeValues))
            {
                contentType = contentTypeValues.FirstOrDefault()!;
            }

            if (response.Headers.TryGetValues(HttpResponseHeader.ContentLength.ToString(), out IEnumerable<string>? contentLengthValues))
            {
                contentLength = ulong.Parse(contentLengthValues.FirstOrDefault()!);
            }

            return await Task.FromResult(HealthCheckResult.Healthy(description: $"Site for images configured at {_option.Value.StorageConfiguration.PublicUrl}")); ;

           
        }
        catch (Exception ex)
        {
            _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
