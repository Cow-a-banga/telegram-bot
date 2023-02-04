using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Commands.WhoAmI
{
    public class GetQuestionsCommand
    {
        private DatabaseContext _db;
        
        public GetQuestionsCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<WhoAmIQuestionDto>> ExecuteAsync()
        {
            var questions = await _db.Questions
                .Where(x => x.PlayDate == null)
                .Include(x => x.PlayerFrom)
                .Include(x => x.PlayerTo)
                .ToListAsync();

            return questions;
        }
    }
}