using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Commands.Commands;
using TelegramBot.Extensions;

namespace TelegramBot.Commands
{
    public abstract class Command
    {
        public string[] Names { get; protected set; }
        public string Description { get; protected set; }
        public CommandGroup CommandGroup { get; protected set; }
        public abstract Task ExecuteAsync(Message message);
        public abstract Task SendAnswer(Message message, ITelegramBotClient botClient);
        public abstract void Clear();

        protected string[] GetParams(string text)
        {
            return text
                .SplitCommand()
                .Skip(1)
                .ToArray();
        }
        
    }
}