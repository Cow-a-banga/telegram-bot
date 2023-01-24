using DataBase.Models;
using Telegram.Bot.Types;

namespace TelegramBot.Extensions
{
    public static class UserExtensions
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.FirstName,
                Lastname = user.LastName,
                Username = user.Username
            };
        }
    }
}