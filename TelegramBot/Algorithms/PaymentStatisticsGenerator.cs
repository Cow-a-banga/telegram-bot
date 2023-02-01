using System.Collections.Generic;
using System.Linq;
using DataBase.Models;
using TelegramBot.Services.Payment;

namespace TelegramBotExperiments.Algorithms
{
    public class PaymentStatisticsGenerator
    {
        public PaymentStatistics GetStatistics(List<PaymentDto> payments)
        {
            return new()
            {
                CommonPayments = GetCommonPayments(payments),
                PersonalPayments = GetPersonalPayments(payments),
            };
        }

        private List<Payment> GetCommonPayments(List<PaymentDto> payments)
        {
            var common = payments
                .Where(x => x.UserToId == null)
                .GroupBy(x => x.UserFromId)
                .Select((g) => new Payment {Amount = g.Sum(x => x.Amount), UserFromId = g.Key})
                .ToList();

            if (common.Count > 0)
            {
                var amount = common.Sum(x => x.Amount);
                var amountPerPesron = amount / common.Count;
                common.ForEach(x => x.Amount = amountPerPesron - x.Amount);
            }

            return common;
        }

        private List<Payment> GetPersonalPayments(List<PaymentDto> payments)
        {
            return payments
                .Where(x => x.UserToId != null)
                .GroupBy(x => new {x.UserFromId, x.UserToId})
                .Select(g =>
                    new Payment
                    {
                        Amount = g.Sum(x => x.Amount), UserFromId = g.Key.UserFromId, UserToId = g.Key.UserToId
                    })
                .ToList();
        }
    }
}