namespace TelegramBot.Payment
{
    public class Payment
    {
        public decimal Amount { get; set; }
        public string Username { get; set; }

        public override string ToString()
        {
            return $"{Username}: {Amount}";
        }
    }
}