using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using TelegramBot.Payment;
using TelegramBotExperiments.Commands;
using TelegramBotExperiments.Commands.Commands;

namespace TelegramBotExperiments
{

    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("5907541674:AAFjPqD9-2DEHbr90ergnnMZLH7hbtj5R-A");
        static CommandService _commandService = new CommandService();
        static PaymentService _paymentService = new PaymentService();
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
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
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
            
            _commandService.Register(new ClearCommand(_paymentService));
            _commandService.Register(new UndoCommand(_paymentService));
            _commandService.Register(new PayCommand(_paymentService));
            _commandService.Register(new StatCommand(_paymentService));
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