using System.Threading.Tasks;
using Common;
using DataBase.Models;

namespace DataBase
{
    public class AddUserIfNotExistCommand
    {
        private DatabaseContext _db;
        private ILogger _logger = new ConsoleLogger();
        
        public AddUserIfNotExistCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task ExecuteAsync(UserDto userDto)
        {
            _logger.Log($"Поиск пользователя по id {userDto.Id}");
            var user = await _db.Users.FindAsync(userDto.Id);
            
            _logger.Log(user == null ? "Пользователь не найден" : "Пользователь найден");

            if (user == null)
            {
                _db.Users.Add(userDto);
                await _db.SaveChangesAsync();
            }
        }
    }
}