using Microsoft.EntityFrameworkCore;

namespace BinanceP2pAlerts.Db;
internal class AppDbContext : DbContext
{
    public DbSet<PriceRecord> PriceRecords { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
