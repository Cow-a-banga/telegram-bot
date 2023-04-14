using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Date;
using DataBase;
using DataBase.Commands;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Extensions;
using TelegramBot.Services.Payment;

namespace TelegramBot.Commands.Commands.Payment
{
    public class ArchiveCommand : Command
    {
        private DatabaseContext _db;
        private List<DebtDto> _debts;
        
        public ArchiveCommand(DatabaseContext db)
        {
            _db = db;
            Names = new[] {"/archive", "/a"};
            Description = "/a, /archive - архивирует текущий чек";
            CommandGroup = CommandGroup.Payment;
        }
        
        public override async Task ExecuteAsync(Message message)
        {
            _debts = await _db.Debts
                .Where(x => x.PayDate == null)
                .Include(x => x.UserFrom)
                .Include(x => x.UserTo)
                .ToListAsync();
            
            var command = new GetCurrentPaymentsCommand(_db);
            var payments = await command.ExecuteAsync(true);
            var date = PermDate.Get();
            payments.ForEach(x =>  x.PayDate = date);
            _db.Payments.UpdateRange(payments);
            await _db.SaveChangesAsync();
        }

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat, "Чек сохранён");

            if (_debts.Count > 0)
            {
                foreach (var group in _debts.GroupBy(x => x.UserFromId))
                {
                    var chatId = group.Key;

                    foreach (var debt in group)
                    {
                        var myInlineKeyboard = new InlineKeyboardMarkup(
                            InlineKeyboardButton.WithCallbackData("Я оплатил", $"{nameof(DebtDto)} {debt.Id}"));
            
                        await botClient.SendTextMessageAsync(chatId, debt.ToString(), replyMarkup: myInlineKeyboard);
                    }
                }
            }
        }

        public override void Clear()
        {
            _debts = null;
        }
    }
}