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

    await _userRepository.AddUserAsync(user);
  }

  public Task<User> GetUserAsync(string login)
  {
    return _userRepository.GetUserAsync(login);
  }
}