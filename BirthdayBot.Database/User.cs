namespace BirthdayBot.Database;

public class User
{
  public int Id { get; set; }
  public string Login { get; set; }
  public string Name { get; set; }
  public Birthday Birthday { get; set; }
}