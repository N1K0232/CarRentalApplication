using CarRentalApplication.DataAccessLayer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarRentalApplication.BusinessLayer.BackgroundServices;

public class DataContextConnectionService : BackgroundService
{
    private PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
    private readonly IServiceProvider services;
    private readonly ILogger<DataContextConnectionService> logger;

    public DataContextConnectionService(IServiceProvider services, ILogger<DataContextConnectionService> logger)
    {
        this.services = services;
        this.logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("starting connection service...");
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("stopping service");
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using (var scope = services.CreateScope())
                {
                    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    await dataContext.Database.OpenConnectionAsync(stoppingToken);
                    await dataContext.Database.CloseConnectionAsync();

                    logger.LogInformation("connection test succeeded");
                }
            }
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "can't connect");
        }
    }

    public override void Dispose()
    {
        timer.Dispose();
        timer = null;

        base.Dispose();
        GC.SuppressFinalize(this);
    }
}