using System.Collections.Generic;
using System.Linq;

namespace TelegramBot.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitCommand(this string text)
        {
            return text
                .Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim());
        }
    }
}