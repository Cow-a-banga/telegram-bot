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
            var groups = CommandService.Commands
                .GroupBy(x => x.CommandGroup)
                .OrderBy(x => x.Key)
                .Select(g => string.Join('\n', g.Select(x => x.Description).OrderBy(x => x)));
            var text = string.Join("\n\n", groups);
            await botClient.SendTextMessageAsync(message.Chat, $"{text}");
        }

        public override void Clear() {}
    }
}