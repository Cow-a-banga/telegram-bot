using System;
using System.Collections.Generic;
using System.Linq;
using TelegramBot.Payment;

namespace TelegramBotExperiments.Algorithms
{
    public static class PaymentAlgorithms
    {
        private const decimal Alpha = 0.0001m;
        
        public static List<Payment> GenerateTransfers(PaymentStatistics statistics)
        {
            var stat = statistics.Clone() as PaymentStatistics;
            var transfers = GenerateCommonTransfers(stat.CommonPayments);
            stat.PersonalPayments.ForEach(x => (x.UserFromId, x.UserToId) = (x.UserToId.Value, x.UserFromId));
            transfers.AddRange(stat.PersonalPayments);
            return transfers
                .GroupBy(x => (x.UserFromId, x.UserToId))
                .Select(g => new Payment
                    {Amount = g.Sum(x => x.Amount), UserFromId = g.Key.UserFromId, UserToId = g.Key.UserToId})
                .FilterUselessPayments()
                .ToList();
        }

        private static IEnumerable<Payment> FilterUselessPayments(this IEnumerable<Payment> payments)
        {
            return payments
                .GroupBy(x => new
                {
                    LessId = Math.Min(x.UserFromId, x.UserToId.Value),
                    LargerId = Math.Max(x.UserFromId, x.UserToId.Value)
                })
                .Select(x =>
                {
                    var lst = x.ToList();
                    if (lst.Count == 1)
                        return lst[0];

                    var amount = Math.Abs(lst[0].Amount.Value - lst[1].Amount.Value);
                    Payment result = lst[0].Amount.Value > lst[1].Amount.Value ? lst[0] : lst[1];
                    result.Amount = amount;
                    return result;
                });
        }
        
        private static List<Payment> GenerateCommonTransfers(List<Payment> payments)
        {
            var result = new List<Payment>();

            var debts = payments.Where(x => x.Amount > 0).ToList();
            var markedDebts = new bool[debts.Count];
            var additions = payments.Where(x => x.Amount < 0).ToList();
            var markedAdditions = new bool[additions.Count];

            for (var i = 0; i < debts.Count; i++)
            {
                for (var j = 0; j < additions.Count; j++)
                {
                    if (!markedAdditions[j] && Math.Abs((debts[i].Amount + additions[j].Amount).Value) < Alpha)
                    {
                        markedAdditions[j] = markedDebts[i] = true;
                        result.Add(new Payment{UserFromId = debts[i].UserFromId, UserToId = additions[j].UserFromId, Amount = debts[i].Amount});
                    }
                }
            }
            
            for (var i = 0; i < debts.Count; i++)
            {
                if(markedDebts[i])
                    continue;
                for (var j = 0; j < additions.Count; j++)
                {
                    if (!markedAdditions[j])
                    {
                        var transferAmount = Math.Min(debts[i].Amount.Value, -additions[j].Amount.Value);
                        additions[j].Amount += transferAmount;
                        debts[i].Amount -= transferAmount;
                        
                        result.Add(new Payment{UserFromId = debts[i].UserFromId, UserToId = additions[j].UserFromId, Amount = transferAmount});

                        if(Math.Abs(additions[j].Amount.Value) < Alpha)
                            markedAdditions[j] = true;

                        if (Math.Abs(debts[i].Amount.Value) < Alpha)
                        {
                            markedDebts[i] = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}