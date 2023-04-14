using System.Linq;
using System.Threading.Tasks;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Commands.Commands.Payment
{
    public class UndoCommand:Command
    {
        private DatabaseContext _db;

        public UndoCommand(DatabaseContext db)
        {
            _db = db;
            Description = "/u, /undo - удаляет последний платёж";
            Names = new[] {"/undo", "/u"};
            CommandGroup = CommandGroup.Payment;
        }
        
        public override void Clear() {}

        public override async Task ExecuteAsync(Message message)
        {
            var payment = await _db.Payments
                .Where(x => x.UserFromId == message.From.Id && x.PayDate == null)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (payment != null)
            {
                _db.Payments.Remove(payment);
                await _db.SaveChangesAsync();
            }
        }

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat, "Сумма отменена");
        }    
    }
}