using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Commands.WhoAmI
{
    public class UpdateQuestionsCommand
    {
        private DatabaseContext _db;
        
        public UpdateQuestionsCommand(DatabaseContext db)
        {
            _db = db;
        }

        public async Task ExecuteAsync(IEnumerable<WhoAmIQuestionDto> questions)
        {
            _db.Questions.UpdateRange(questions);
            await _db.SaveChangesAsync();
        }
    }
}