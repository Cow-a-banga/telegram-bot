using System.Collections.Generic;
using System.Linq;

namespace TelegramBot.Payment
{
    public class PaymentService
    {
        private readonly LinkedList<Payment> _payments = new();

        public void AddPayment(Payment p) => _payments.AddLast(p);

        public void Undo()
        {
            if(_payments.Count > 0)
                _payments.RemoveLast();
        }

        public void Clear() => _payments.Clear();

        public PaymentStatistics GetStat()
        {
            return new()
            {
                CommonPayments = GetCommonPayments(),
                PersonalPayments = GetPersonalPayments(),
            };
        }

        private List<Payment> GetCommonPayments()
        {
            var common = _payments
                .Where(x => x.UserTo == null)
                .GroupBy(x => x.UserFrom)
                .Select((g) => new Payment {Amount = g.Sum(x => x.Amount), UserFrom = g.Key})
                .ToList();
            
            if (common.Count > 0)
            {
                var amount = common.Sum(x => x.Amount);
                var amountPerPesron = amount / common.Count;
                common.ForEach(x => x.Amount = amountPerPesron - x.Amount);
            }

            return common;
        }
        
        private List<Payment> GetPersonalPayments()
        {
            return _payments
                .Where(x => x.UserTo != null)
                .GroupBy(x => new {x.UserFrom, x.UserTo})
                .Select(g =>
                    new Payment
                    {
                        Amount = g.Sum(x => x.Amount), UserFrom = g.Key.UserFrom, UserTo = g.Key.UserTo
                    })
                .ToList();
        }
    }
}