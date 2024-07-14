namespace BirthdayBot.Exceptions;

public class UserAlreadyExistsException() : Exception(DefaultMessage)
{
  private const string DefaultMessage = "User already exists.";
}