using System.Threading.Tasks;
using DataBase.Models;

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

            var newQuestion = new WhoAmIQuestionDto {PlayerFromId = userId};
            _db.Questions.Add(newQuestion);
            await _db.SaveChangesAsync();
        }
    }
}