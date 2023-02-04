using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Commands;
using TelegramBot.Extensions;

namespace TelegramBotExperiments.Commands
{
    public class CommandService
    {
        private static List<Command> _commands = new List<Command>();

        public static IEnumerable<Command> Commands => _commands;

        public async Task ExecuteAsync(Message message, ITelegramBotClient botClient)
        {
            var prefix = message.Text.SplitCommand().FirstOrDefault();
            var prefixParts = prefix.Split('@').ToArray();
            prefix = prefixParts.Length == 2 && prefixParts[1] == "zadrotovpermibot" ? prefixParts[0] : prefix;
            prefix = prefix.ToLower();
            foreach (var command in _commands)
            {
                if (command.Names.Contains(prefix))
                {
                    await command.ExecuteAsync(message);
                    await command.SendAnswer(message, botClient);
                    command.Clear();
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