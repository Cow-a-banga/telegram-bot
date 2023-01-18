using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Payment;
using User = TelegramBot.Payment.User;

namespace TelegramBotExperiments.Commands.Commands
{
    public class PayCommand :  Command
    {
        private PaymentService _paymentService;
        private Payment _payment;

        public PayCommand(PaymentService paymentService)
        {
            _paymentService = paymentService;
            Description = "/p, /pay (сумма) [@username] - добавляет платёж человека в чек (можно закинуть не в общак, а кокретному человеку)";
            Names = new[] {"/pay", "/p"};
        }

        public override void Execute(Message message)
        {
            var parameters = GetParams(message.Text);
            _payment = new Payment
            {
                UserFrom =
                {
                    Firstname = message.From.FirstName,
                    Lastname = message.From.LastName,
                    Username = message.From.Username
                }
            };


            foreach (var parameter in parameters)
            {
                if (decimal.TryParse(parameter, out var amount) && !_payment.Amount.HasValue)
                {
                    _payment.Amount = amount;
                    continue;
                }

                if (parameter[0] == '@' && _payment.UserTo == null)
                {
                    _payment.UserTo = new User {Username = parameter[1..]};
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