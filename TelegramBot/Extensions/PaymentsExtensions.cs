using System.Collections.Generic;
using System.Linq;
using TelegramBot.Payment;

namespace TelegramBot.Extensions
{
    public static class PaymentsExtensions
    {
        public static string JoinLines(this IEnumerable<PaymentInputDto> payments)
        {
            return string.Join('\n', payments.Select(x => x.ToString()));
        }
    }
}