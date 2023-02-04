using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase;
using DataBase.Commands.WhoAmI;
using DataBase.Models;
using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Commands.Commands.WhoAmI
{
    enum WishStatus
    {
        Success,
        NoText,
        NotStarted,
        TimeToStart,
        NotInGame,
    }
    
    public class WishCommand: Command
    {
        private DatabaseContext _db;
        private List<WhoAmIQuestionDto> questions;
        private WishStatus _status;

        public WishCommand(DatabaseContext db)
        {
            _db = db;
            Names = new[] { "/wish", "/w" };
            Description = @"/w, /wish (текст) - загадать в игре 'Кто я'";
            CommandGroup = CommandGroup.WhoAmI;
        }

        public override async Task ExecuteAsync(Message message)
        {
            var parameters = GetParams(message.Text);

            if (parameters.Length == 0)
            {
                _status = WishStatus.NoText;
                return;
            }

            var question = await new GetQuestionByFromIdCommand(_db).ExecuteAsync(message.From.Id);

            if (question == null)
            {
                _status = WishStatus.NotInGame;
                return;
            }
            
            if (!question.PlayerToId.HasValue)
            {
                _status = WishStatus.NotStarted;
                return;
            }
            
            var text = string.Join(' ', parameters);
            question.Text = text;
            await new UpdateQuestionsCommand(_db).ExecuteAsync(new[] {question});

            questions = await new GetQuestionsCommand(_db).ExecuteAsync();
            if (questions.All(x => !string.IsNullOrEmpty(x.Text)))
            {
                _status = WishStatus.TimeToStart;
                await new ArchiveQuestionsCommand(_db).ExecuteAsync();
            }
            
        }

        public override async Task SendAnswer(Message message, ITelegramBotClient botClient)
        {
            switch (_status)
            {
                case WishStatus.Success:
                    await botClient.SendTextMessageAsync(message.Chat, "Когда все загадают, вы увидите список загаданных");
                    break;
                case WishStatus.NoText:
                    await botClient.SendTextMessageAsync(message.Chat, "Вы не написали что вы загадываете");
                    break;
                case WishStatus.NotStarted:
                    await botClient.SendTextMessageAsync(message.Chat, "Игра ещё не началась");
                    break;
                case WishStatus.NotInGame:
                    await botClient.SendTextMessageAsync(message.Chat, "Вы не в игре. Введите /ready");
                    break;
                case WishStatus.TimeToStart:
                    foreach (var question in questions)
                    {
                        await botClient.SendTextMessageAsync(question.PlayerFromId, GetResultFor(question.PlayerFromId));
                    }
                    break;
            }

        }

        private string GetResultFor(long userId)
        {
            var lines = questions
                .Where(x => x.PlayerToId != userId)
                .Select(x => $"{x.PlayerTo.Name}: {x.Text}")
                .OrderBy(x => x);
            
            return string.Join('\n', lines);
        }

        public override void Clear()
        {
            _status = WishStatus.Success;
        }
    }
}