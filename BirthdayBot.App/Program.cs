using BirthdayBot;
using BirthdayBot.App.Adapters;
using BirthdayBot.App.Handlers;
using BirthdayBot.App.Models;
using BirthdayBot.App.Settings;
using BirthdayBot.Database;
using BirthdayBot.Database.Initialization;
using NLog.Web;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

var botConfigSection = builder.Configuration.GetSection("BotConfig");
var botConfig = botConfigSection.Get<BotConfig>();

builder.Services
 .AddOptions()
 .Configure<BotConfig>(botConfigSection);

builder.Services
 .AddScoped<IUserRepository, UserRepository>()
 .AddScoped<IUserService, UserService>()
 .AddScoped<ITelegramBotAdapter, TelegramBotAdapter>()
 .AddScoped<IUpdateHandler, UpdateHandler>()
 .AddDataContext();

builder.Services.AddHttpClient("telegram_bot_client")
 .AddTypedClient<ITelegramBotClient>(httpClient =>
  {
    var options = new TelegramBotClientOptions(botConfig.BotToken);
    return new TelegramBotClient(options, httpClient);
  });

builder.Services.AddAutoMapper(
  typeof(DbMappingProfiles),
  typeof(AppMappingProfiles));

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Host.UseNLog(new NLogAspNetCoreOptions()
{
  ReplaceLoggerFactory = true
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapControllers();

app.Run();