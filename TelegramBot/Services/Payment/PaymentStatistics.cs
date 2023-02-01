using System;
using System.Collections.Generic;

namespace TelegramBot.Payment
{
    public class PaymentStatistics: ICloneable
    {
        public List<Payment> CommonPayments { get; set; }
        public List<Payment> PersonalPayments { get; set; }
        public object Clone()
        {
            var newCommons = new List<Payment>(CommonPayments.Count);
            CommonPayments.ForEach((item)=>
            {
                newCommons.Add(item.Clone() as Payment);
            });
            
            var newPersonals = new List<Payment>(PersonalPayments.Count);
            PersonalPayments.ForEach((item)=>
            {
                newPersonals.Add(item.Clone() as Payment);
            });
            
            return new PaymentStatistics
            {
                CommonPayments = newCommons,
                PersonalPayments = newPersonals,
            };
        }
    }
}