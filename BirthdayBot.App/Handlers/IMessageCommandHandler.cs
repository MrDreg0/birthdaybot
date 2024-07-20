using Telegram.Bot.Types;

namespace BirthdayBot.App.Handlers;

public interface IMessageCommandHandler
{
  Task OnMessageCommand(Message msg);
}