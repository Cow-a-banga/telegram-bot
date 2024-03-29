﻿using System.Linq;
using System.Threading.Tasks;
using DataBase;
using DataBase.Commands;
using DataBase.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Commands.Commands.Payment
{
    public class PayCommand :  Command
    {
        private Services.Payment.Payment? _payment;
        private DatabaseContext _db;

        public PayCommand(DatabaseContext db)
        {
            _db = db;
            Description = "/p, /pay (сумма) [@username] - добавляет платёж человека в чек (можно закинуть не в общак, а кокретному человеку)";
            Names = new[] {"/pay", "/p"};
            CommandGroup = CommandGroup.Payment;
        }

        public override async Task ExecuteAsync(Message message)
        {
            var parameters = GetParams(message.Text);
            _payment = new Services.Payment.Payment
            {
                UserFromId = message.From.Id,
            };


            foreach (var parameter in parameters)
            {
                if (decimal.TryParse(parameter, out var amount) && !_payment.Amount.HasValue)
                {
                    _payment.Amount = amount;
                    continue;
                }

                if (parameter[0] == '@' && _payment.UserToId == null)
                {
                    var command = new FindUserByUsernameCommand(_db);
                    var user = await command.ExecuteAsync(parameter[1..]);
                    _payment.UserToId = user.Id;
                    continue;
                }
            }

            if (_payment.Amount.HasValue)
            {
                var dto = _payment.ToDto();

                _db.Payments.Add(dto);
                
                if (dto.UserToId != null && !_db.Payments.Any(x => x.UserFromId == dto.UserFromId && x.UserToId == null && x.PayDate == null))
                {
                    _db.Payments.Add(new PaymentDto{Amount = 0, UserFromId = dto.UserFromId});
                }
                
                await _db.SaveChangesAsync();
            }
        }

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            if(_payment != null && _payment.Amount.HasValue)
                await botClient.SendTextMessageAsync(message.Chat, "Сумма добавлена");
            else
                await botClient.SendTextMessageAsync(message.Chat, "Некорректная сумма");
        }

        public override void Clear()
        {
            _payment = null;
        }
    }
}