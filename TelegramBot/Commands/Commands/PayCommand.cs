using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Payment;

namespace TelegramBotExperiments.Commands.Commands
{
    public class PayCommand :  Command
    {
        private PaymentService _paymentService;
        private bool success;

        public PayCommand(PaymentService paymentService)
        {
            _paymentService = paymentService;
            Description = "/p, /pay [сумма] - добавляет платёж человека в чек";
            Names = new[] {"/pay", "/p"};
        }

        public override void Execute(Message message)
        {
            var parameters = GetParams(message.Text);
            var amountStr = parameters.FirstOrDefault();
            
            success = decimal.TryParse(amountStr, out var amount);
            
            if(success)
                _paymentService.AddPayment(new Payment{Amount = amount, Username = $"{message.From.FirstName} {message.From.LastName}"});
        }

        public override async void SendAnswer(Message message, ITelegramBotClient botClient)
        {
            if(success)
                await botClient.SendTextMessageAsync(message.Chat, "Сумма добавлена");
            else
                await botClient.SendTextMessageAsync(message.Chat, "Некорректная сумма");
        }
    }
}