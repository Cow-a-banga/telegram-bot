using System;
using System.Collections.Generic;

namespace TelegramBot.Services.Payment
{
    public class PaymentStatistics: ICloneable
    {
        public List<Services.Payment.Payment> CommonPayments { get; set; }
        public List<Services.Payment.Payment> PersonalPayments { get; set; }
        public object Clone()
        {
            var newCommons = new List<Services.Payment.Payment>(CommonPayments.Count);
            CommonPayments.ForEach((item)=>
            {
                newCommons.Add(item.Clone() as Services.Payment.Payment);
            });
            
            var newPersonals = new List<Services.Payment.Payment>(PersonalPayments.Count);
            PersonalPayments.ForEach((item)=>
            {
                newPersonals.Add(item.Clone() as Services.Payment.Payment);
            });
            
            return new PaymentStatistics
            {
                CommonPayments = newCommons,
                PersonalPayments = newPersonals,
            };
        }
    }
}