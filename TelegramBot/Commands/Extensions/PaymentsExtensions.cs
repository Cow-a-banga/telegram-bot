using System.Collections.Generic;
using System.Linq;
using TelegramBot.Payment;

namespace TelegramBotExperiments.Commands.Extensions
{
    public static class PaymentsExtensions
    {
        public static string JoinLines(this IEnumerable<Payment> payments)
        {
            return string.Join('\n', payments.Select(x => x.ToString()));
        }
    }
}