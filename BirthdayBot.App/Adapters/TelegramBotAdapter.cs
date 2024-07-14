using AutoMapper;
using BirthdayBot.App.Models;
using Domain = BirthdayBot.Models;

namespace BirthdayBot.App.Adapters;

public class TelegramBotAdapter : ITelegramBotAdapter
{
  private readonly IUserService _userService;
  private readonly IMapper _mapper;

  public TelegramBotAdapter(IUserService userService, IMapper mapper)
  {
    _userService = userService;
    _mapper = mapper;
  }

  public async Task AddUserAsync(Participant participant)
  {
    var domainUser = _mapper.Map<Domain.User>(participant);

    await _userService.AddUserAsync(domainUser);
  }

  public async Task<Participant> GetUserAsync(string login)
  {
    var domainUser = await _userService.GetUserAsync(login);

    return _mapper.Map<Participant>(domainUser);
  }

  public Task UpdateUserAsync(string login, string name, string birthday)
  {
    var domainBirthday = _mapper.Map<Domain.Birthday>(birthday);

    return _userService.UpdateUserAsync(login, name, domainBirthday);
  }
}