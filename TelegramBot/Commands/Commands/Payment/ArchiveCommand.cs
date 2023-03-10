using System;
using System.Threading.Tasks;
using Common.Date;
using DataBase;
using DataBase.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Extensions;

namespace TelegramBot.Commands.Commands.Payment
{
    public class ArchiveCommand : Command
    {
        private DatabaseContext _db;
        
        public ArchiveCommand(DatabaseContext db)
        {
            _db = db;
            Names = new[] {"/archive", "/a"};
            Description = "/a, /archive - архивирует текущий чек";
            CommandGroup = CommandGroup.Payment;
        }
        
        public override async Task ExecuteAsync(Message message)
        {
            var command = new GetCurrentPaymentsCommand(_db);
            var payments = await command.ExecuteAsync();
            var date = PermDate.Get();
            payments.ForEach(x =>  x.PayDate = date);
            _db.Payments.UpdateRange(payments);
            await _db.SaveChangesAsync();
        }

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat, "Чек сохранён");
        }
        
        public override void Clear() {}
    }
}