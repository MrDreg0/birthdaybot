using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BirthdayBot.App.Handlers;

public class UpdateHandler(
  IMessageCommandHandler messageCommandHandler,
  ILogger<UpdateHandler> logger)
  : IUpdateHandler
{
  public async Task HandleUpdateAsync(Update update)
  {
    await (update switch
    {
      { Type: UpdateType.Message, Message: { } message } => messageCommandHandler.OnMessageCommand(message),
      _ => UnknownCommandHandlerAsync(update)
    });
  }

  private Task UnknownCommandHandlerAsync(Update update)
  {
    logger.LogWarning("Unknown update type {UpdateType}.", update.Type);

    return Task.CompletedTask;
  }
}