namespace BirthdayBot.Exceptions;

public class UserNotFoundException() : Exception(DefaultMessage)
{
  private const string DefaultMessage = "User not found.";
}