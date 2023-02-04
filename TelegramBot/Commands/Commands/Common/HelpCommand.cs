using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotExperiments.Commands;

namespace TelegramBot.Commands.Commands.Common
{
    public class HelpCommand:Command
    {
        public HelpCommand()
        {
            Description = "/h, /help - список команд";
            Names = new[] {"/help", "/h"};
            CommandGroup = CommandGroup.Common;
        }
        public override Task ExecuteAsync(Message message) {return  Task.CompletedTask;}

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            var text = string.Join('\n', CommandService.Commands.Select(x => x.Description).OrderBy(x => x));
            await botClient.SendTextMessageAsync(message.Chat, $"{text}");
        }

        public override void Clear() {}
    }
}