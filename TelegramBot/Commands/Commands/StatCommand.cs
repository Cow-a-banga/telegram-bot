using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase;
using DataBase.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Extensions;
using TelegramBot.Payment;
using TelegramBotExperiments.Algorithms;

namespace TelegramBotExperiments.Commands.Commands
{
    public class StatCommand:Command
    {
        private PaymentService _paymentService;
        private PaymentStatistics _statistics;
        private List<Payment> _transfers;
        private DatabaseContext _db;

        public StatCommand(PaymentService paymentService, DatabaseContext db)
        {
            _paymentService = paymentService;
            _db = db;
            Description = "/s, /stat - показывает кто сколько должен отдать или получить";
            Names = new[] {"/stat", "/s"};
        }
        

        public override async Task ExecuteAsync(Message message)
        {
            _statistics = _paymentService.GetStat();
            _transfers = PaymentAlgorithms.GenerateTransfers(_statistics);
        }

        public override async void SendAnswer(Message message, ITelegramBotClient botClient)
        {
            var usersId =
                _statistics.CommonPayments.Select(x => x.UserFromId)
                    .Union(_statistics.CommonPayments.Where(x => x.UserToId.HasValue).Select(x => x.UserToId.Value))
                    .Union(_statistics.PersonalPayments.Where(x => x.UserToId.HasValue).Select(x => x.UserToId.Value))
                    .Union(_statistics.PersonalPayments.Select(x => x.UserFromId));

            var command = new FindUsersByIdCommand(_db);
            var users = (await command.ExecuteAsync(usersId)).ToDictionary(x => x.Id, x=>x);
            
            if (_statistics.CommonPayments.Count == 0 && _statistics.PersonalPayments.Count == 0)
            {
                await botClient.SendTextMessageAsync(message.Chat, $"Чек пуст");
                return;
            }

            string commonText = null, personalText = null, transferText = null;
            
            if (_statistics.CommonPayments.Count > 0)
            {
                commonText = _statistics.CommonPayments
                    .Select(x => x.ToDto<PaymentOutputDto>(users))
                    .JoinLines();
            }

            if (_statistics.PersonalPayments.Count > 0)
            {
                personalText = _statistics.PersonalPayments
                    .Select(x => x.ToDto<PaymentInputDto>(users))
                    .JoinLines();
            }

            if (_transfers.Count > 0)
            {
                transferText = _transfers
                    .Select(x => x.ToDto<PaymentOutputDto>(users))
                    .JoinLines();
            }
            
            await botClient.SendTextMessageAsync(message.Chat, $"+ платит, - получает:\n{commonText}\n\n{personalText}");
            if (transferText != null)
            {
                await botClient.SendTextMessageAsync(message.Chat, $"Итого:\n{transferText}");

                foreach (var group in _transfers.GroupBy(x => x.UserToId))
                {
                    if (!group.Key.HasValue) 
                        continue;
                    
                    var text = group
                        .Select(x => x.ToDto<PaymentOutputDto>(users))
                        .JoinLines();
                    var chat = await botClient.GetChatAsync(group.Key);
                    await botClient.SendTextMessageAsync(chat, $"Итого:\n{text}");
                }
            }
        }
    }
}