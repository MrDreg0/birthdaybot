using Microsoft.EntityFrameworkCore;

namespace BirthdayBot.Database;

public class DataContext : DbContext
{
  public DbSet<User> Users { get; set; }
  public DbSet<Birthday> Birthdays { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseInMemoryDatabase("BirthdayBot");
  }
}