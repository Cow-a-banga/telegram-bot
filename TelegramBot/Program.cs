using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using DataBase;
using DataBase.Models;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using TelegramBot.Common.CollectCommands;
using TelegramBot.Extensions;
using TelegramBotExperiments.Commands;

namespace TelegramBotExperiments
{

    class Program
    {
        private static string token = Environment.GetEnvironmentVariable("TGBOT_TOKEN");
        private static ITelegramBotClient bot = new TelegramBotClient(token);
        private static DatabaseContext _context = new ();
        private static CommandService _commandService = new (CommandsCollector.CollectAll());
        private static ILogger _logger = new ConsoleLogger();
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case Telegram.Bot.Types.Enums.UpdateType.Message:
                        await HandleMessage(botClient, update);
                        break;
                    case Telegram.Bot.Types.Enums.UpdateType.CallbackQuery:
                        await HandleCallbackQuery(botClient, update);
                        break;
                        
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                await botClient.SendTextMessageAsync(update.Message.Chat, "Что-то сломалось!", cancellationToken: cancellationToken);
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        static void Main(string[] args)
        {
            var culture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Thread.Sleep(Timeout.Infinite);
        }

        private static async Task HandleMessage(ITelegramBotClient botClient, Update update)
        {
            var command = new AddUserIfNotExistCommand(_context);
            await command.ExecuteAsync(update.Message.From.ToDto());
                    
            if (update.Message?.Text == null)
                return;
                    
            _logger.Log($"Запрос {update.Message.Text} от {update.Message.From.FirstName} получен");

            await _commandService.ExecuteAsync(update.Message, botClient);
        }
        
        
        private static async Task HandleCallbackQuery(ITelegramBotClient botClient, Update update)
        {
            var parameters = update.CallbackQuery.Data.Split(' ');

            switch (parameters[0])
            {
                case nameof(DebtDto):
                    await botClient.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat, update.CallbackQuery.Message.MessageId);
                    
                    var id = long.Parse(parameters[1]);
                    var debt = await _context.Debts.FindAsync(id);
                    debt.Payed = true;
                    _context.Debts.Update(debt);
                    await _context.SaveChangesAsync();
                    break;
            }

            //
            //TODO: Добавить личные сообщения при archive, для каждого добавлять кнопку с id Debt. При нажатии на кнопку отмечать Debt как оплаченный, удалять кнопку кодом ниже и отправлять получателю уведомление о том, что деньги отправлены. Архивировать строки в Debts.
            //
            //
        }
    }
}