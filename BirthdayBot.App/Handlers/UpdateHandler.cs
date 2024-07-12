using BirthdayBot.App.Adapters;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BirthdayBot.App.Handlers;

public class UpdateHandler(
  ITelegramBotClient botClient,
  ITelegramBotAdapter botAdapter)
  : IUpdateHandler
{
  private const string AddBirthdayCommand = "/addbirthday";
  private const string GetBirthdayCommand = "/getbirthday";

  public async Task HandleUpdateAsync(Update update)
  {
    await (update switch
    {
      { Message: { } message } => OnMessage(message),
      _ => UnknownUpdateHandlerAsync(update)
    });
  }

  private async Task OnMessage(Message msg)
  {
    switch (msg.Text)
    {
      case { } messageText:
        await (messageText.Split(' ')[0] switch
        {
          AddBirthdayCommand => OnAddBirthday(msg),
          GetBirthdayCommand => OnGetBirthday(msg),
          _ => Usage(msg)
        });
        break;
    }
  }

  private async Task<Message> Usage(Message msg)
  {
    const string usage =
      $"""
         <b>Bot menu</b>:
          {AddBirthdayCommand} - add birthday
          {GetBirthdayCommand} - get birthday
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

    await botAdapter.AddUserAsync(user);

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

  private static Task UnknownUpdateHandlerAsync(Update _)
  {
    return Task.CompletedTask;
  }
}