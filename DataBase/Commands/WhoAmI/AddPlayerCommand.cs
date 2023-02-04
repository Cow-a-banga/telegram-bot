using System.Linq;
using System.Threading.Tasks;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Commands.WhoAmI
{
    public class AddPlayerCommand
    {
        private DatabaseContext _db;

        public AddPlayerCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task ExecuteAsync(long userId)
        {
            var question = await new GetQuestionByFromIdCommand(_db).ExecuteAsync(userId);

            if (question != null) return;

            _db.Questions.Add(new WhoAmIQuestionDto {PlayerFromId = userId});
            await _db.SaveChangesAsync();
        }
    }
}