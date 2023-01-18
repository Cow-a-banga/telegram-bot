using System.Collections.Generic;

namespace TelegramBot.Payment
{
    public class PaymentStatistics
    {
        public List<Payment> CommonPayments { get; set; }
        public List<Payment> PersonalPayments { get; set; }
    }
}