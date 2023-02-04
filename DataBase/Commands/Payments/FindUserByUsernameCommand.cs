using System.Linq;
using System.Threading.Tasks;
using Common;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Commands
{
    public class FindUserByUsernameCommand
    {
        private DatabaseContext _db;
        private ILogger _logger = new ConsoleLogger();

        public FindUserByUsernameCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<UserDto> ExecuteAsync(string username)
        {
            _logger.Log($"Поиск пользователя по username {username}");
            var user = await _db.Users.Where(x => x.Username == username).FirstOrDefaultAsync();

            _logger.Log(user == null ? "Пользователь не найден" : "Пользователь найден");

            return user;
        }
    }
}