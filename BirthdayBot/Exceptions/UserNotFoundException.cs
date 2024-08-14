namespace BirthdayBot.Exceptions;

public class UserNotFoundException(string login) : Exception
{
  private const string MessageTemplate = "User with login '{0}' not found.";

  public string ErrorMessage { get; } = string.Format(MessageTemplate, login);
}