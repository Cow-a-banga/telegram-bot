namespace TelegramBot.Services.WhoAmI
{
    public class WhoAmIQuestion
    {
        public long PlayerFromId { get; set; }
        public long PlayerToId { get; set; }
        public string Word { get; set; }
    }
}