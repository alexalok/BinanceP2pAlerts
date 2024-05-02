using BinanceP2pAlerts.Db;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace BinanceP2pAlerts;

public class Worker(ILogger<Worker> _logger, IServiceProvider _services) : BackgroundService
{
    bool _isThresholdTriggered;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var executor = ActivatorUtilities.CreateInstance<IterationExecutor>(scope.ServiceProvider);
            try
            {
                var isThresholdTriggered = await executor.ExecuteIteration(_isThresholdTriggered);
                _isThresholdTriggered = isThresholdTriggered;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during iteration!");
                throw;
            }
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    class IterationExecutor(ILogger<IterationExecutor> _logger, BinanceClient _binance, ITelegramBotClient _telegram, IOptions<AppSettings> appSettings, DbClient _db)
    {
        readonly AppSettings _appSettings = appSettings.Value;

        public async Task<bool> ExecuteIteration(bool isThresholdTriggered)
        {
            var ad = await _binance.GetFirstAd();
            var minPrice = ad.Price;
            _logger.LogInformation("Min price: {MinPrice}", minPrice);

            await _db.RecordMinPrice(minPrice);

            if (minPrice < _appSettings.NotifyIfPriceIsBelow)
            {
                if (!isThresholdTriggered)
                    await _telegram.SendTextMessageAsync(_appSettings.ChatId, $"Price is now below threshold: {minPrice}");
                return true;
            }
            else
            {
                if (isThresholdTriggered)
                    await _telegram.SendTextMessageAsync(_appSettings.ChatId, $"Price is now above threshold: {minPrice}");
                return false;
            }
        }
    }
}
