using System;
using System.Collections.Generic;

namespace TelegramBot.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            var rnd = new Random();
            for (var i = 0; i < list.Count; i++)
            {
                var j = rnd.Next(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}