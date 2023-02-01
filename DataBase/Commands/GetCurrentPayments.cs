using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Commands
{
    public class GetCurrentPaymentsCommand
    {
        private DatabaseContext _db;

        public GetCurrentPaymentsCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<PaymentDto>> ExecuteAsync()
        {
            return await _db.Payments
                .Where(x => x.PayDate == null)
                .ToListAsync();
        }
    }
}