using System.Threading.Tasks;
using DataBase;
using DataBase.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Commands.Commands.Payment
{
    public class ClearCommand:Command
    {
        private DatabaseContext _db;

        public ClearCommand(DatabaseContext db)
        {
            _db = db;
            Description = "/c, /clear - очищает текущий чек";
            Names = new[] {"/clear", "/c"};
            CommandGroup = CommandGroup.Payment;
        }
        
        public override async Task ExecuteAsync(Message message)
        {
            var command = new GetCurrentPaymentsCommand(_db);
            var payments = await command.ExecuteAsync();
            _db.RemoveRange(payments);
            await _db.SaveChangesAsync();
        }

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat, "Сумма очищенна");
        }
        
        public override void Clear() {}
    }
}