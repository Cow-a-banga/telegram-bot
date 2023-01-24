namespace TelegramBot.Payment
{
    public class PaymentOutputDto:PaymentInputDto
    {
        public override string ToString()
        {
            return UserTo == null ? $"{UserFrom}: {Amount:C2}" : $"{UserFrom} должен ({UserTo}) {Amount:C2}";
        }
    }
}