using System.Globalization;
using Domain = BirthdayBot.Models;
using AutoMapper;

namespace BirthdayBot.App.Models;

public class AppMappingProfiles : Profile
{
  public AppMappingProfiles()
  {
    CreateMap<Participant, Domain.User>()
      .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday))
      .ReverseMap()
      .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday.BirthDate.ToString("dd/MM/yyyy")));

    CreateMap<string, Domain.Birthday>()
      .ForMember(dst => dst.BirthDate,
        opt => opt.MapFrom(src => DateTime.ParseExact(src, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
  }
}