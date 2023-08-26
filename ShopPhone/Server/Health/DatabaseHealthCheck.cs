
using log4net;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Request;
using System.Reflection;

namespace ShopPhone.Server.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private IUserService _userService = null!;
    private ILog _logger = null!;
    public DatabaseHealthCheck(IUserService userService, ILog logger)
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
                UserName = "HealthUser",
                Password = "HealthPassword"
            };

            var response = await _userService.LoginAsync(dummyUser);

            _logger.Info($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}");

            if (response.ErrorMessage!.ToLower().Contains("Usuario".ToLower()))
            {
                return await Task.FromResult(HealthCheckResult.Healthy("Db response OK"));
            }
            else
            {
                return await Task.FromResult(HealthCheckResult.Unhealthy(response.ErrorMessage));
            }

        }
        catch (Exception ex)
        {
            _logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            return HealthCheckResult.Unhealthy(exception: ex);
        }

    }
}
