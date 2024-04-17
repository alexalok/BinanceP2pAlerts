using System.Text.Json.Serialization;

namespace BinanceP2pAlerts;
[JsonSerializable(typeof(BinanceResponse))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
