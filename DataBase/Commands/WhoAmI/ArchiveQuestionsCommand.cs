using System.Threading.Tasks;
using Common.Date;
using DataBase;
using DataBase.Commands.WhoAmI;
using DataBase.Models;

namespace TelegramBot.Commands.Commands.WhoAmI
{
    public class ArchiveQuestionsCommand
    {
        private DatabaseContext _db;

        public ArchiveQuestionsCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task ExecuteAsync()
        {
            var questions = await new GetQuestionsCommand(_db).ExecuteAsync();

            var date = PermDate.Get();
            
            foreach (var question in questions)
            {
                question.PlayDate = date;
            }

            await new UpdateQuestionsCommand(_db).ExecuteAsync(questions);
        }
    }
}