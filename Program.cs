using BinanceP2pAlerts;
using BinanceP2pAlerts.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptionsWithValidateOnStart<AppSettings>().Bind(builder.Configuration);
builder.Services.AddSingleton<IValidateOptions<AppSettings>, AppSettingsValidator>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite("Data Source=data/db.sqlite");
});
builder.Services.AddTransient<DbClient>();
builder.Services.AddHostedService<DbMigrator>();

builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClient<BinanceClient>(h =>
{
    h.BaseAddress = new("https://p2p.binance.com/");
});
builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(s =>
{
    var appSettings = s.GetRequiredService<IOptions<AppSettings>>().Value;
    return new TelegramBotClient(appSettings.TelegramBotToken);
});

var host = builder.Build();
host.Run();
