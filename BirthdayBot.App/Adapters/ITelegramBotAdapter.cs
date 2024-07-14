using BirthdayBot.App.Models;

namespace BirthdayBot.App.Adapters;

public interface ITelegramBotAdapter
{
  Task AddUserAsync(Participant participant);

  Task<Participant> GetUserAsync(string login);

  Task UpdateUserAsync(string login, string name, string birthday);
}