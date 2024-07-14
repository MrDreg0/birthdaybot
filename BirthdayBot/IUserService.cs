using BirthdayBot.Models;

namespace BirthdayBot;

public interface IUserService
{
  Task AddUserAsync(User user);

  Task<User> GetUserAsync(string login);

  Task UpdateUserAsync(string login, string name, Birthday birthday);
}