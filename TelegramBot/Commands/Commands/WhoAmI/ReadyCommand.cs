using System.Threading.Tasks;
using DataBase;
using DataBase.Commands.WhoAmI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Commands.Commands.WhoAmI
{
    public class ReadyCommand: Command
    {
        private DatabaseContext _db;

        public ReadyCommand(DatabaseContext db)
        {
            _db = db;
            Names = new[] { "/ready", "/r" };
            Description = @"/r, /ready - вы готовы к игре в 'Кто я'";
            CommandGroup = CommandGroup.WhoAmI;
        }

        public override async Task ExecuteAsync(Message message)
        {
            var command = new AddPlayerCommand(_db);
            await command.ExecuteAsync(message.From.Id);
        }

        public override void Clear() {}

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat, "Вы готовы к игре, введите /start, когда все будут готовы");
        }
    }
}