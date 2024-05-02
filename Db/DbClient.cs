namespace BinanceP2pAlerts.Db;
internal class DbClient(AppDbContext _db)
{
    public async Task RecordMinPrice(decimal minPrice)
    {
        _db.PriceRecords.Add(new()
        {
            MinPrice = minPrice,
            RecordedAt = DateTimeOffset.UtcNow
        });
        await _db.SaveChangesAsync();
    }
}
