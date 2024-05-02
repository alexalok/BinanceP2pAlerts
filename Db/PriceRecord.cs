namespace BinanceP2pAlerts.Db;
internal class PriceRecord
{
    public int Id { get; set; }
    public DateTimeOffset RecordedAt { get; set; }
    public decimal MinPrice { get; set; }
}
