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
            return UserTo == null ? $"{UserFrom}: {Amount.Value.ToString("C2", CultureInfo.CurrentCulture)}" : $"{UserTo} должен ({UserFrom}) {Amount:C}";
        }

        public object Clone()
        {
            return new Payment {Amount = Amount, UserFrom = UserFrom, UserTo = UserTo};
        }
    }
}