using BirthdayBot.App.Adapters;
using BirthdayBot.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BirthdayBot.App.Handlers;

public class MessageTextCommandHandler(
  ITelegramBotClient botClient,
  ITelegramBotAdapter botAdapter,
  ILogger<MessageTextCommandHandler> logger) : IMessageTextCommandHandler
{
  private const string AddBirthdayCommand = "/addbirthday";
  private const string GetBirthdayCommand = "/getbirthday";
  private const string UpdateBirthdayCommand = "/updatebirthday";

  public async Task OnMessageTextCommand(Message msg)
  {
    if (msg.Text is null)
    {
      logger.LogWarning("Message with id {MessageId} has no text.", msg.MessageId);

      return;
    }

    await (msg.Text.Split(' ')[0] switch
    {
      AddBirthdayCommand => OnAddBirthday(msg),
      GetBirthdayCommand => OnGetBirthday(msg),
      UpdateBirthdayCommand => OnUpdateBirthday(msg),
      _ => Usage(msg)
    });
  }

  private async Task<Message> Usage(Message msg)
  {
    const string usage =
      $"""
         <b>Bot menu</b>:
          {AddBirthdayCommand} - add birthday
          {GetBirthdayCommand} - get birthday
          {UpdateBirthdayCommand} - update birthday
       """;

    return await botClient.SendTextMessageAsync(
      msg.Chat,
      usage,
      parseMode: ParseMode.Html,
      replyMarkup: new ReplyKeyboardRemove());
  }

  private async Task<Message> OnAddBirthday(Message msg)
  {
    if (msg.Text is not { } messageText)
    {
      return await Usage(msg);
    }

    var parts = messageText.Split(' ');

    if (parts.Length is not 5)
    {
      return await AddBirthdayUsage(msg);
    }

    var user = new Models.Participant
    {
      Login = parts[1],
      Name = parts[2] + parts[3],
      Birthday = parts[4]
    };

    try
    {
      await botAdapter.AddUserAsync(user);
    }
    catch (UserAlreadyExistsException ex)
    {
      return await botClient.SendTextMessageAsync(msg.Chat.Id, ex.Message + " " + "For update use /updatebirthday");
    }

    return await botClient.SendTextMessageAsync(msg.Chat.Id, "Birthday added!");
  }

  private async Task<Message> AddBirthdayUsage(Message msg)
  {
    const string usage =
      $"""
         <b>Add Birthday usage</b>:
         {AddBirthdayCommand} <i>login</i> <i>name</i> <i>lastname</i> <i>dd/mm/yyyy</i>
        
         <b>Example</b>:
           <code>{AddBirthdayCommand} johndoe John Doe 01/01/2000</code>
       """;

    return await botClient.SendTextMessageAsync(
      msg.Chat,
      usage,
      parseMode: ParseMode.Html,
      replyMarkup: new ReplyKeyboardRemove());
  }

  private async Task<Message> OnGetBirthday(Message msg)
  {
    if (msg.Text is not { } messageText)
    {
      return await Usage(msg);
    }

    var parts = messageText.Split(' ');

    if (parts.Length is not 2)
    {
      return await GetBirthdayUsage(msg);
    }

    var birthday = await botAdapter.GetUserAsync(parts[1]);

    return await botClient.SendTextMessageAsync(
      msg.Chat.Id,
      $"Birthday {parts[1]} is {birthday.Birthday}");
  }

  private async Task<Message> GetBirthdayUsage(Message msg)
  {
    const string usage =
      $"""
         <b>Get Birthday usage</b>:
         {GetBirthdayCommand} <i>login</i>
        
         <b>Example</b>:
           <code>{GetBirthdayCommand} johndoe</code>
       """;

    return await botClient.SendTextMessageAsync(
      msg.Chat,
      usage,
      parseMode: ParseMode.Html,
      replyMarkup: new ReplyKeyboardRemove());
  }

  private async Task<Message> OnUpdateBirthday(Message msg)
  {
    if (msg.Text is not { } messageText)
    {
      return await Usage(msg);
    }

    var parts = messageText.Split(' ');

    if (parts.Length is not 5)
    {
      return await UpdateBirthdayUsage(msg);
    }

    try
    {
      await botAdapter.UpdateUserAsync(parts[1], parts[2] + parts[3], parts[4]);
    }
    catch (UserNotFoundException ex)
    {
      return await botClient.SendTextMessageAsync(msg.Chat.Id, ex.Message + " " + "For add use /addbirthday");
    }

    return await botClient.SendTextMessageAsync(msg.Chat.Id, "Birthday updated!");
  }

  private async Task<Message> UpdateBirthdayUsage(Message msg)
  {
    const string usage =
      $"""
         <b>Update Birthday usage</b>:
         {UpdateBirthdayCommand} <i>login</i> <i>name</i> <i>lastname</i> <i>dd/mm/yyyy</i>
        
         <b>Attention</b>:
           User with <i>login</i> should exist.
        
         <b>Example</b>:
           <code>{UpdateBirthdayCommand} johndoe John Doe 01/01/2000</code>
       """;

    return await botClient.SendTextMessageAsync(
      msg.Chat,
      usage,
      parseMode: ParseMode.Html,
      replyMarkup: new ReplyKeyboardRemove());
  }
}