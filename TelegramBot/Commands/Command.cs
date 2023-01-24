using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Extensions;

namespace TelegramBotExperiments.Commands
{
    public abstract class Command
    {
        public string[] Names { get; protected set; }
        public string Description { get; protected set; }
        public abstract Task ExecuteAsync(Message message);
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