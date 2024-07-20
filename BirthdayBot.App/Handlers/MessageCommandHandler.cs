using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BirthdayBot.App.Handlers;

public class MessageCommandHandler(
  ILogger<MessageCommandHandler> logger,
  IMessageTextCommandHandler messageTextCommandHandler) : IMessageCommandHandler
{
  public async Task OnMessageCommand(Message msg)
  {
    await (msg.Type switch
    {
      MessageType.Text => messageTextCommandHandler.OnMessageTextCommand(msg),
      _ => UnknownMessageCommandHandlerAsync(msg)
    });
  }

  private Task UnknownMessageCommandHandlerAsync(Message msg)
  {
    logger.LogWarning("Unknown message type {MessageType}.", msg.Type);

    return Task.CompletedTask;
  }
}