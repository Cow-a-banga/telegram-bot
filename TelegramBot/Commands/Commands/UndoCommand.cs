using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Payment;

namespace TelegramBotExperiments.Commands.Commands
{
    public class UndoCommand:Command
    {
        private PaymentService _paymentService;

        public UndoCommand(PaymentService paymentService)
        {
            _paymentService = paymentService;
            Description = "/u, /undo - удаляет последний платёж";
            Names = new[] {"/undo", "/u"};
        }
        

        public override void Execute(Message message)
        {
            _paymentService.Undo();
        }

        public override async void SendAnswer(Message message, ITelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat, "Сумма отменена");
        }    
    }
}