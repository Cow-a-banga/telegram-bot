using System.Collections.Generic;
using System.Linq;

namespace TelegramBot.Payment
{
    public class PaymentService
    {
        private LinkedList<Payment> _payments = new LinkedList<Payment>();

        public void AddPayment(Payment p) => _payments.AddLast(p);

        public void Undo()
        {
            if(_payments.Count > 0)
                _payments.RemoveLast();
        }

        public void Clear() => _payments.Clear();

        public List<Payment> GetStat()
        {
            return _payments
                .GroupBy(x => x.Username)
                .Select((g) => new Payment {Amount = g.Sum(x => x.Amount), Username = g.Key})
                .ToList();
        }
    }
}