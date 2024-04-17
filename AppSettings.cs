using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace BinanceP2pAlerts;
internal class AppSettings
{
    [Required]
    public long ChatId { get; set; }

    [Required]
    public decimal NotifyIfPriceIsBelow { get; set; }

    [Required]
    public string TelegramBotToken { get; set; } = null!;
}

[OptionsValidator]
partial class AppSettingsValidator : IValidateOptions<AppSettings>
{

}