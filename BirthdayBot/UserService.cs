using BirthdayBot.Exceptions;
using BirthdayBot.Models;

namespace BirthdayBot;

public class UserService(IUserRepository userRepository) : IUserService
{
  public async Task AddUserAsync(User user)
  {
    // TODO добавить валидацию

    var existingUser = await userRepository.ExistsUserAsync(user.Login);

    if (existingUser)
    {
      throw new UserAlreadyExistsException();
    }

    await userRepository.AddUserAsync(user);
  }

  public async Task<User> GetUserAsync(string login)
  {
    var user = await userRepository.TryGetUserAsync(login);

    if (user is null)
    {
        throw new UserNotFoundException(login);
    }

    return user;
  }

  public async Task UpdateUserAsync(string login, string name, Birthday birthday)
  {
    var existingUser = await userRepository.ExistsUserAsync(login);

    if (!existingUser)
    {
      throw new UserNotFoundException(login);
    }

    await userRepository.UpdateUserAsync(login, name, birthday);
  }
}