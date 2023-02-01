using DataBase.Models;

namespace TelegramBot.Services.Payment
{
    public class PaymentInputDto
    {
        public decimal? Amount { get; set; }
        public UserDto UserFrom { get; set; }
        public UserDto? UserTo { get; set; }

        public override string ToString()
        {
            return UserTo == null ? $"{UserFrom}: {Amount:C2}" : $"{UserFrom} заплатил за ({UserTo}) {Amount:C2}";
        }
    }
}