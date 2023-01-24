using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Commands
{
    public class FindUsersByIdCommand
    {
        private DatabaseContext _db { get; set; }
        private ILogger _logger = new ConsoleLogger();

        public FindUsersByIdCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<UserDto>> ExecuteAsync(IEnumerable<long> userIds)
        {
            var result = new List<UserDto>();
            foreach (var id in userIds)
            {
                _logger.Log($"Поиск пользователя по id {id}");
                var user = await _db.Users.FindAsync(id);
                _logger.Log(user == null ? "Пользователь не найден" : "Пользователь найден");

                if (user != null)
                {
                    result.Add(user);
                }
            }
            

            return result;
        }
    }
}