using System;
using System.Threading.Tasks;
using DataBase;
using DataBase.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Payment;

namespace TelegramBotExperiments.Commands.Commands
{
    public class PayCommand :  Command
    {
        private PaymentService _paymentService;
        private Payment _payment;
        private DatabaseContext _db;

        public PayCommand(PaymentService paymentService, DatabaseContext db)
        {
            _paymentService = paymentService;
            _db = db;
            Description = "/p, /pay (сумма) [@username] - добавляет платёж человека в чек (можно закинуть не в общак, а кокретному человеку)";
            Names = new[] {"/pay", "/p"};
        }

        public override async Task ExecuteAsync(Message message)
        {
            var parameters = GetParams(message.Text);
            _payment = new Payment
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
            
            
            
            if(_payment.Amount.HasValue)
                _paymentService.AddPayment(_payment);
        }

        public override async void SendAnswer(Message message, ITelegramBotClient botClient)
        {
            if(_payment.Amount.HasValue)
                await botClient.SendTextMessageAsync(message.Chat, "Сумма добавлена");
            else
                await botClient.SendTextMessageAsync(message.Chat, "Некорректная сумма");
        }
    }
}