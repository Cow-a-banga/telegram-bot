using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using TelegramBot.Payment;
using TelegramBotExperiments.Algorithms;
using TelegramBotExperiments.Commands;
using TelegramBotExperiments.Commands.Commands;
using TelegramBotExperiments.Commands.Extensions;
using User = TelegramBot.Payment.User;

namespace TelegramBotExperiments
{

    class Program
    {
        private static ITelegramBotClient bot = new TelegramBotClient("5907541674:AAFjPqD9-2DEHbr90ergnnMZLH7hbtj5R-A");
        private static DatabaseContext _context = new DatabaseContext();
        private static CommandService _commandService = new CommandService();
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                var command = new AddUserIfNotExistCommand(_context);
                command.Execute(update.Message.From.ToDto());
                
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    var message = update.Message;

                    if (message?.Text == null)
                        return;

                    _commandService.Execute(message, botClient);
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
            
            _context.Database.Migrate();
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
            
            var paymentService = new PaymentService();
            _commandService.Register(new ClearCommand(paymentService));
            _commandService.Register(new UndoCommand(paymentService));
            _commandService.Register(new PayCommand(paymentService));
            _commandService.Register(new StatCommand(paymentService));
            _commandService.Register(new HelpCommand());

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