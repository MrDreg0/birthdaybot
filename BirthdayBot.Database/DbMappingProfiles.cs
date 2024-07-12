using Domain = BirthdayBot.Models;
using AutoMapper;

namespace BirthdayBot.Database;

public class DbMappingProfiles : Profile
{
  public DbMappingProfiles()
  {
    CreateMap<User, Domain.User>()
      .ReverseMap();

    CreateMap<Birthday, Domain.Birthday>()
      .ReverseMap();
  }
}