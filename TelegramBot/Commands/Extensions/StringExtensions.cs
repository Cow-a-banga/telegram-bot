using System.Collections.Generic;
using System.Linq;

namespace TelegramBotExperiments.Commands.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitCommand(this string text)
        {
            return text
                .ToLower()
                .Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim());
        }
    }
}