using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using TelegramBot.Commands;

namespace TelegramBot.Common.CollectCommands
{
    public static class CommandsCollector
    {
        public static List<Command> CollectAll()
        {
            var context = new DatabaseContext();
            return Assembly.GetAssembly(typeof(Command))
                .GetTypes()
                .Where(x => x.IsAssignableTo(typeof(Command)) && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x, context) as Command)
                .ToList();
        }
    }
}