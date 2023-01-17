using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Payment;

namespace TelegramBotExperiments.Commands.Commands
{
    public class ClearCommand:Command
    {
        private PaymentService _paymentService;

        public ClearCommand(PaymentService paymentService)
        {
            _paymentService = paymentService;
            Description = "/c, /clear - очищает текущий чек";
            Names = new[] {"/clear", "/c"};
        }
        

        public override void Execute(Message message)
        {
            _paymentService.Clear();
        }

        public override async void SendAnswer(Message message, ITelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat, "Сумма очищенна");
        }
    }
}