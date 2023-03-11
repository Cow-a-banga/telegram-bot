using System.Linq;
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
            if (dto.UserToId != null)
            {
                var payment = _db.Payments.FirstOrDefault(x => x.UserFromId == dto.UserFromId && x.UserToId == null && x.PayDate == null);
                if (payment == null)
                {
                    _db.Payments.Add(new PaymentDto{Amount = 0, UserFromId = dto.UserFromId});
                }
            }
            
            _db.Payments.Add(dto);
            await _db.SaveChangesAsync();
        }
    }
}