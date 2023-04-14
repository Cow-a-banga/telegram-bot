using System.Collections.Generic;
using System.Linq;
using DataBase.Models;
using TelegramBot.Services.Payment;

namespace TelegramBot.Extensions
{
    public static class PaymentsExtensions
    {
        public static string JoinLines(this IEnumerable<PaymentInputDto> payments)
        {
            return string.Join("\n\n", payments.Select(x => x.ToString()));
        }
        public static IEnumerable<DebtDto> ToDebtDto(this IEnumerable<Payment> payments)
        {
            return payments.Select(x => new DebtDto
            {
                Amount = x.Amount.Value,
                Payed = false,
                UserFromId = x.UserFromId,
                UserToId = x.UserToId,
            });
        }
    }
}