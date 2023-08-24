using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using ShopPhone.Shared.Entities;
using System.Reflection;
using log4net;

namespace ShopPhone.Server.Health;

public class DirectoryHealthCheck : IHealthCheck
{
    private IOptions<AppConfig> _option;
    private ILog _logger;

    public DirectoryHealthCheck(IOptions<AppConfig> option, ILog logger)
    {
        _option = option;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        //_options.Value.SmtpConfiguration
        try
        {
            if (!Directory.Exists(_option.Value.StorageConfiguration.Path))
            {
                return HealthCheckResult.Unhealthy(description: $"No existe {_option.Value.StorageConfiguration.Path}");
            }

            return await Task.FromResult(HealthCheckResult.Healthy(description: $"Existe el Directorio {_option.Value.StorageConfiguration.Path}"));
        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
