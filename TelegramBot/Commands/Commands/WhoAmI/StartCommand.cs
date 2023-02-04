using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataBase;
using DataBase.Commands.WhoAmI;
using DataBase.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Extensions;


namespace TelegramBot.Commands.Commands.WhoAmI
{
    enum StartStatus
    {
        Success,
        NotEnoughPlayers
    }
    
    public class StartCommand : Command
    {
        private DatabaseContext _db;
        private StartStatus _status = StartStatus.Success;
        private List<WhoAmIQuestionDto> questions;
        
        public StartCommand(DatabaseContext db)
        {
            _db = db;
            Description = "/start - начинает игру в 'Кто я' (должно быть хотя бы 2 человека)";
            Names = new[] {"/start"};
            CommandGroup = CommandGroup.WhoAmI;
        }
        
        public override async Task ExecuteAsync(Message message)
        {
            questions = await new GetQuestionsCommand(_db).ExecuteAsync();
            if (questions.Count <= 1)
            {
                _status = StartStatus.NotEnoughPlayers;
                return;
            }

            questions.Shuffle();

            for (var i = 0; i < questions.Count - 1; i++)
            {
                questions[i].PlayerToId = questions[i + 1].PlayerFromId;
                questions[i].PlayerTo = questions[i + 1].PlayerFrom;
            }

            questions[^1].PlayerToId = questions[0].PlayerFromId;
            questions[^1].PlayerTo = questions[0].PlayerFrom;

            var command = new UpdateQuestionsCommand(_db);
            await command.ExecuteAsync(questions);
        }
        

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            switch (_status)
            {
                case StartStatus.Success:
                    foreach (var question in questions)
                    {
                        await botClient.SendTextMessageAsync(question.PlayerFromId, $"Ты загадываешь игроку {question.PlayerTo.Name}. Загадай кто он через /wish");
                    }
                    break;
                case StartStatus.NotEnoughPlayers:
                    await botClient.SendTextMessageAsync(message.Chat, "Слишком мало людей для начала");
                    break;
            }
        }

        public override void Clear()
        {
            _status = StartStatus.Success;
        }
    }
}