
using Serilog;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Request;
using System.Reflection;


namespace ShopPhone.Server.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private IUserService _userService = null!;
    private ILogger<DatabaseHealthCheck> _logger = null!;
    public DatabaseHealthCheck(IUserService userService, ILogger<DatabaseHealthCheck> logger)
    {
        _userService = userService;
        _logger = logger;
    }


    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                    CancellationToken cancellationToken = default)
    {

        try
        {

            var dummyUser = new LoginRequestDTO()
            {
                UserName = "DatabaseHealthCheck",
                Password = "123456*"
            };

            var response = await _userService.LoginAsync(dummyUser);
          
            if ( response.Success)
            {
                _logger.LogInformation($"HealthCheckResult.Healthy Db response OK - {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}");
                return await Task.FromResult(HealthCheckResult.Healthy("Db response OK"));
            }
            else
            {
                _logger.LogInformation($"HealthCheckResult.Unhealthy {response.ErrorMessage} - {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}");
                return await Task.FromResult(HealthCheckResult.Unhealthy(response.ErrorMessage));
            }
           

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error - {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return HealthCheckResult.Unhealthy(exception: ex);
        }

    }
}
