namespace TelegramBot.Payment
{
    public class Payment
    {
        public decimal? Amount { get; set; }
        public User UserFrom { get; set; } = new User();
        public User? UserTo { get; set; }

        public override string ToString()
        {
            return UserTo == null ? $"{UserFrom}: {Amount:C}" : $"{UserTo} должен ({UserFrom}) {Amount:C}";
        }
    }
}