using BirthdayBot.Exceptions;
using BirthdayBot.Models;

namespace BirthdayBot;

public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;

  public UserService(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task AddUserAsync(User user)
  {
    // TODO добавить валидацию

    var existingUser = await _userRepository.ExistsUserAsync(user.Login);

    if (existingUser)
    {
      throw new UserAlreadyExistsException();
    }

    await _userRepository.AddUserAsync(user);
  }

  public Task<User> GetUserAsync(string login)
  {
    return _userRepository.GetUserAsync(login);
  }

  public async Task UpdateUserAsync(string login, string name, Birthday birthday)
  {
    var existingUser = await _userRepository.ExistsUserAsync(login);

    if (!existingUser)
    {
      throw new UserNotFoundException();
    }

    await _userRepository.UpdateUserAsync(login, name, birthday);
  }
}