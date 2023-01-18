using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Payment;
using TelegramBotExperiments.Algorithms;

namespace TelegramBotExperiments.Commands.Commands
{
    public class StatCommand:Command
    {
        private PaymentService _paymentService;
        private PaymentStatistics _statistics;
        private List<Payment> _transfers;

        public StatCommand(PaymentService paymentService)
        {
            _paymentService = paymentService;
            Description = "/s, /stat - показывает кто сколько должен отдать или получить";
            Names = new[] {"/stat", "/s"};
        }
        

        public override void Execute(Message message)
        {
            _statistics = _paymentService.GetStat();
            _transfers = PaymentAlgorithms.GenerateTransfers(_statistics);
        }

        public override async void SendAnswer(Message message, ITelegramBotClient botClient)
        {
            if (_statistics.CommonPayments.Count == 0 && _statistics.PersonalPayments.Count == 0)
            {
                await botClient.SendTextMessageAsync(message.Chat, $"Чек пуст");
                return;
            }

            string commonText = null, personalText = null, transferText = null;
            
            if (_statistics.CommonPayments.Count > 0)
            {
                commonText = string.Join('\n', _statistics.CommonPayments.Select(x => x.ToString()));
            }

            if (_statistics.PersonalPayments.Count > 0)
            {
                personalText = string.Join('\n', _statistics.PersonalPayments.Select(x => x.ToString()));
            }

            if (_transfers.Count > 0)
            {
                transferText = string.Join('\n', _transfers.Select(x => x.ToString()));
            }
            
            await botClient.SendTextMessageAsync(message.Chat, $"+ платит, - получает:\n{commonText}\n\n{personalText}");
            if(transferText != null)
                await botClient.SendTextMessageAsync(message.Chat, $"Итого:\n{transferText}");
        }
    }
}