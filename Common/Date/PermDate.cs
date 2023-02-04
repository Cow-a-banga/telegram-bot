using System;

namespace Common.Date
{
    public static class PermDate
    {
        public static DateTime Get()
        {
            return DateTime.Now.ToUniversalTime().AddHours(5);
        }
    }
}