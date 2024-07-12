using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Domain = BirthdayBot.Models;

namespace BirthdayBot.Database;

public class UserRepository : IUserRepository
{
  private readonly DataContext _context;
  private readonly IMapper _mapper;

  public UserRepository(DataContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task AddUserAsync(Domain.User user)
  {
    var dbUser = _mapper.Map<User>(user);

    await _context.Users.AddAsync(dbUser);
    await _context.SaveChangesAsync();
  }

  public async Task<Domain.User> GetUserAsync(string login)
  {
    var user = await _context.Users
      .Include(u => u.Birthday)
      .FirstOrDefaultAsync(u => u.Login == login);

    return _mapper.Map<Domain.User>(user);
  }
}