using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace BinanceP2pAlerts;

public class Worker(ILogger<Worker> _logger, IServiceProvider _services) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var executor = ActivatorUtilities.CreateInstance<IterationExecutor>(scope.ServiceProvider);
            await executor.ExecuteIteration();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    class IterationExecutor(ILogger<IterationExecutor> _logger, BinanceClient _binance, ITelegramBotClient _telegram, IOptions<AppSettings> appSettings)
    {
        readonly AppSettings _appSettings = appSettings.Value;

        public async Task ExecuteIteration()
        {
            var ad = await _binance.GetFirstAd();
            var minPrice = ad.Price;
            _logger.LogInformation("Min price: {MinPrice}", minPrice);

            if (minPrice < _appSettings.NotifyIfPriceIsBelow)
                await _telegram.SendTextMessageAsync(_appSettings.ChatId, $"Min price: {minPrice}");
        }
    }
}
