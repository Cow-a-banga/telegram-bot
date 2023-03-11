using System.Collections.Generic;
using System.Threading.Tasks;
using DataBase;
using DataBase.Commands.WhoAmI;
using DataBase.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Commands.Commands.WhoAmI
{
    public class DropCommand: Command
    {
        private DatabaseContext _db;
        private List<WhoAmIQuestionDto> _questions;

        public DropCommand(DatabaseContext db)
        {
            _db = db;
            Names = new[] { "/drop", "/d" };
            Description = @"/d, /drop - сбросить игру в 'Кто я'";
            CommandGroup = CommandGroup.WhoAmI;
        }

        public override async Task ExecuteAsync(Message message)
        {
            var command = new GetQuestionsCommand(_db);
            _questions = await command.ExecuteAsync();
            _db.Questions.RemoveRange(_questions);
            await _db.SaveChangesAsync();
        }

        public override void Clear()
        {
            _questions = null;
        }

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            foreach (var question in _questions)
            {
                await botClient.SendTextMessageAsync(question.PlayerFromId, "Игра в 'Кто я' отменена");
            }
        }
    }
}