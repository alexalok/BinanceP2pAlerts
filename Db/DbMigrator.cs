using Microsoft.EntityFrameworkCore;

namespace BinanceP2pAlerts.Db;
internal class DbMigrator(IServiceProvider _services) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync(CancellationToken.None);
    }
}
