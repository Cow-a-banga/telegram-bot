using System.Linq;
using System.Xml.Schema;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotExperiments.Commands.Commands
{
    public class HelpCommand:Command
    {
        public HelpCommand()
        {
            Description = "/h, /help - список команд";
            Names = new[] {"/help", "/h"};
        }
        public override void Execute(Message message) {}

        public override async void SendAnswer(Message message, ITelegramBotClient botClient)
        {
            var text = string.Join('\n', CommandService.Commands.Select(x => x.Description).OrderBy(x => x));
            await botClient.SendTextMessageAsync(message.Chat, $"{text}");
        }
    }
}