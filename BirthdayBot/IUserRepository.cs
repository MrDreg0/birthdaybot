﻿using BirthdayBot.Models;

namespace BirthdayBot;

public interface IUserRepository
{
  Task AddUserAsync(User user);

  Task<User> GetUserAsync(string login);

  Task<bool> ExistsUserAsync(string login);

  Task UpdateUserAsync(string login, string name, Birthday birthday);
}