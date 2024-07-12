using BirthdayBot.Models;

namespace BirthdayBot;

public interface IUserRepository
{
  Task AddUserAsync(User user);

  Task<User> GetUserAsync(string login);
}