using System.Threading.Tasks;
using DataBase.Models;

namespace DataBase
{
    public class AddUserIfNotExistCommand
    {
        private DatabaseContext _db { get; set; }
        
        public AddUserIfNotExistCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task Execute(UserDto userDto)
        {
            var user = await _db.Users.FindAsync(userDto.Id);

            if (user == null)
            {
                _db.Users.Add(userDto);
                await _db.SaveChangesAsync();
            }
        }
    }
}