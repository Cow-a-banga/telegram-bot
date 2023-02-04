using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Commands.WhoAmI
{
    public class GetQuestionByFromIdCommand
    {
        private DatabaseContext _db;
                
        public GetQuestionByFromIdCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<WhoAmIQuestionDto?> ExecuteAsync(long userId)
        {
            var question = await _db.Questions
                .Where(x => x.PlayerFromId == userId && x.PlayDate == null)
                .FirstOrDefaultAsync();

            return question;
        }
    }
}