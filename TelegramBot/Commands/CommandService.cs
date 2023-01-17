using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotExperiments.Commands.Extensions;

namespace TelegramBotExperiments.Commands
{
    public class CommandService
    {
        private static List<Command> _commands = new List<Command>();

        public static IEnumerable<Command> Commands => _commands;

        public void Execute(Message message, ITelegramBotClient botClient)
        {
            var prefix = message.Text.SplitCommand().FirstOrDefault();
            var prefixParts = prefix.Split('@').ToArray();
            prefix = prefixParts.Length == 2 && prefixParts[1] == "zadrotovpermibot" ? prefixParts[0] : prefix;
            foreach (var command in _commands)
            {
                if (command.Names.Contains(prefix))
                {
                    command.Execute(message);
                    command.SendAnswer(message, botClient);
                    return;
                }
            }
        }
        
        public void Register(Command command)
        {
            _commands.Add(command);
        }
    }
}