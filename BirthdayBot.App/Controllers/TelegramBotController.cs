using BirthdayBot.App.Handlers;
using BirthdayBot.App.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayBot.App.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelegramBotController(
  ITelegramBotClient botClient,
  IUpdateHandler updateHandler,
  IOptions<BotConfig> botConfig)
  : ControllerBase
{
  private readonly BotConfig _botConfig = botConfig.Value;

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] Update update)
  {
    if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != _botConfig.SecretToken)
    {
      return Ok();
    }

    if (update?.Message?.Text != null)
    {
      await updateHandler.HandleUpdateAsync(update);
    }

    return Ok();
  }

  [HttpPost("webhook")]
  public async Task<IActionResult> SetWebhook([FromBody] string webhookUrl)
  {
    await botClient.SetWebhookAsync(webhookUrl, secretToken: _botConfig.SecretToken);

    return Ok($"Webhook set to '{webhookUrl}'");
  }

  [HttpGet("webhook")]
  public async Task<ActionResult<WebhookInfo>> GetWebhookInfo()
  {
    var webhookInfo = await botClient.GetWebhookInfoAsync();

    return Ok(webhookInfo);
  }

  [HttpDelete("webhook")]
  public async Task<IActionResult> DeleteWebhook()
  {
    await botClient.DeleteWebhookAsync();

    return Ok("Webhook deleted");
  }
}