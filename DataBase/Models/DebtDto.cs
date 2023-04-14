namespace DataBase.Models
{
    public class DebtDto: PaymentDto
    {
        public bool Payed { get; set; }
        
        public override string ToString()
        {
            return UserTo == null ? $"{UserFrom}: {Amount:C2}" : $"{UserFrom} должен ({UserTo}) {Amount:C2}\nНомер: {UserTo.Phone}\nБанк: {UserTo.BankName}";
        }
    }
}