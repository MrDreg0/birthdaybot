using AutoMapper;
using BirthdayBot.App.Models;
using Microsoft.AspNetCore.Mvc;
using Domain = BirthdayBot.Models;

namespace BirthdayBot.App.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
  private readonly IUserService _userService;
  private readonly IMapper _mapper;

  public UserController(IUserService userService, IMapper mapper)
  {
    _userService = userService;
    _mapper = mapper;
  }

  [HttpPost]
  public async Task<IActionResult> CreateUser([FromBody] Participant participant)
  {
    var domainUser = _mapper.Map<Domain.User>(participant);

    await _userService.AddUserAsync(domainUser);

    return Ok();
  }

  [HttpGet("{login}")]
  public async Task<ActionResult<Participant>> GetUser(string login)
  {
    var domainUser = await _userService.GetUserAsync(login);

    return _mapper.Map<Participant>(domainUser);
  }

  [HttpPut("{login}")]
  public async Task<IActionResult> UpdateUser([FromBody] PatchParticipant patchParticipant, string login)
  {
    var domainBirthday = _mapper.Map<Domain.Birthday>(patchParticipant.Birthday);

    await _userService.UpdateUserAsync(login, patchParticipant.Name, domainBirthday);

    return Ok();
  }
}