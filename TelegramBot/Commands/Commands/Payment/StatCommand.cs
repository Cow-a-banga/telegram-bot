using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataBase;
using DataBase.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Extensions;
using TelegramBot.Services.Payment;
using TelegramBotExperiments.Algorithms;

namespace TelegramBot.Commands.Commands.Payment
{
    public class StatCommand:Command
    {
        private PaymentStatistics _statistics;
        private List<Services.Payment.Payment> _transfers;
        private DatabaseContext _db;
        private ILogger _logger = new ConsoleLogger();

        public StatCommand(DatabaseContext db)
        {
            _db = db;
            Description = "/s, /stat - показывает кто сколько должен отдать или получить";
            Names = new[] {"/stat", "/s"};
        }
        
        public override async Task ExecuteAsync(Message message)
        {
            var command = new GetCurrentPaymentsCommand(_db);
            var payments = await command.ExecuteAsync();
            var generator = new PaymentStatisticsGenerator();
            _statistics = generator.GetStatistics(payments);
            _transfers = TransferGenerator.GenerateTransfers(_statistics);
        }

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
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
            await botClient.SendTextMessageAsync(message.Chat, $"Трансферы:\n{transferText}");
            if (_transfers.Count > 0)
            {
                foreach (var group in _transfers.GroupBy(x => x.UserFromId))
                {
                    var text = group
                        .Select(x => x.ToDto<PaymentOutputDto>(users))
                        .JoinLines();
                    _logger.Log(text);
                    var chat = await botClient.GetChatAsync(group.Key);
                    _logger.Log(group.Key.ToString());
                    _logger.Log((chat.FirstName));
                    await botClient.SendTextMessageAsync(chat, $"Итого:\n{text}");
                }
            }
        }
    }
}