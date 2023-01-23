using System;
using System.Collections.Generic;
using System.Linq;
using TelegramBot.Payment;

namespace TelegramBotExperiments.Algorithms
{
    public class PaymentAlgorithms
    {
        private const decimal Alpha = 0.0001m;
        
        public static List<Payment> GenerateTransfers(PaymentStatistics statistics)
        {
            var stat = statistics.Clone() as PaymentStatistics;
            var transfers = GenerateCommonTransfers(stat.CommonPayments);
            transfers.AddRange(stat.PersonalPayments);
            return transfers
                .GroupBy(x => (x.UserFrom, x.UserTo))
                .Select(g => new Payment
                    {Amount = g.Sum(x => x.Amount), UserFrom = g.Key.UserTo, UserTo = g.Key.UserFrom})
                .ToList();
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
                    if (!markedAdditions[j] && Math.Abs((debts[i].Amount + additions[i].Amount).Value) < Alpha)
                    {
                        markedAdditions[j] = markedDebts[i] = true;
                        result.Add(new Payment{UserFrom = debts[i].UserFrom, UserTo = additions[j].UserFrom, Amount = debts[i].Amount});
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
                        
                        result.Add(new Payment{UserFrom = debts[i].UserFrom, UserTo = additions[j].UserFrom, Amount = transferAmount});

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