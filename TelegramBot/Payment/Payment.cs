using System;
using System.Globalization;

namespace TelegramBot.Payment
{
    public class Payment: ICloneable
    {
        public decimal? Amount { get; set; }
        public User UserFrom { get; set; } = new User();
        public User? UserTo { get; set; }

        public override string ToString()
        {
            return UserTo == null ? $"{UserFrom}: {Amount:C2}" : $"{UserTo} должен ({UserFrom}) {Amount:C2}";
        }

        public object Clone()
        {
            return new Payment {Amount = Amount, UserFrom = UserFrom, UserTo = UserTo};
        }
    }
}