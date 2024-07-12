using Telegram.Bot.Types;

namespace BirthdayBot.App.Handlers;

public interface IUpdateHandler
{
  Task HandleUpdateAsync(Update update);
}