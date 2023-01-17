using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Payment;

namespace TelegramBotExperiments.Commands.Commands
{
    public class StatCommand:Command
    {
        private PaymentService _paymentService;
        private List<Payment> _statistics;

        public StatCommand(PaymentService paymentService)
        {
            _paymentService = paymentService;
            Description = "/s, /stat - показывает кто сколько должен отдать или получить";
            Names = new[] {"/stat", "/s"};
        }
        

        public override void Execute(Message message)
        {
            _statistics = _paymentService.GetStat();
            if (_statistics.Count > 0)
            {
                var amount = _statistics.Sum(x => x.Amount);
                var amountPerPesron = amount / _statistics.Count;
                _statistics.ForEach(x => x.Amount = amountPerPesron - x.Amount);
            }
        }

        public override async void SendAnswer(Message message, ITelegramBotClient botClient)
        {
            if (_statistics.Count > 0)
            {
                var statStr = string.Join('\n', _statistics.Select(x => x.ToString()));
                await botClient.SendTextMessageAsync(message.Chat, $"+ платит, - получает:\n{statStr}");
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat, $"Чек пуст");
            }
        }
    }
}