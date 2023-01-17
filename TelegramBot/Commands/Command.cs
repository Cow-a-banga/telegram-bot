using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotExperiments.Commands.Extensions;

namespace TelegramBotExperiments.Commands
{
    public abstract class Command
    {
        public string[] Names { get; protected set; }
        public string Description { get; protected set; }
        public abstract void Execute(Message message);
        public abstract void SendAnswer(Message message, ITelegramBotClient botClient);

        protected string[] GetParams(string text)
        {
            return text
                .SplitCommand()
                .Skip(1)
                .ToArray();
        }
        
    }
}