using Microsoft.Extensions.DependencyInjection;

namespace BirthdayBot.Database.Initialization;

public static class InitializationExtensions
{
  public static IServiceCollection AddDataContext(this IServiceCollection services)
  {
    services.AddDbContext<DataContext>();

    return services;
  }
}