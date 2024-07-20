using Telegram.Bot.Types;

namespace BirthdayBot.App.Handlers;

public interface IMessageTextCommandHandler
{
  Task OnMessageTextCommand(Message msg);
}