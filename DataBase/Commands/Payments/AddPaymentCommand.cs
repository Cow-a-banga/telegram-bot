using System.Threading.Tasks;
using DataBase.Models;

namespace DataBase.Commands
{
    public class AddPaymentCommand
    {
        private DatabaseContext _db;

        public AddPaymentCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task ExecuteAsync(PaymentDto dto)
        {
            _db.Payments.Add(dto);
            await _db.SaveChangesAsync();
        }
    }
}