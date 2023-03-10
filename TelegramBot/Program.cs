using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Common;
using DataBase;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using TelegramBot.Commands;
using TelegramBot.Commands.Commands.Common;
using TelegramBot.Commands.Commands.Payment;
using TelegramBot.Commands.Commands.WhoAmI;
using TelegramBot.Common.CollectCommands;
using TelegramBot.Extensions;
using TelegramBotExperiments.Commands;

namespace TelegramBotExperiments
{

    class Program
    {
        private static string token = Environment.GetEnvironmentVariable("TGBOT_TOKEN");
        private static ITelegramBotClient bot = new TelegramBotClient(token);
        private static DatabaseContext _context = new DatabaseContext();
        private static CommandService _commandService = new CommandService(CommandsCollector.CollectAll());
        private static ILogger _logger = new ConsoleLogger();
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                var command = new AddUserIfNotExistCommand(_context);
                await command.ExecuteAsync(update.Message.From.ToDto());
                
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    var message = update.Message;

                    if (message?.Text == null)
                        return;
                    
                    _logger.Log($"Запрос {update.Message.Text} от {update.Message.From.FirstName} получен");

                    await _commandService.ExecuteAsync(message, botClient);
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
    }
}