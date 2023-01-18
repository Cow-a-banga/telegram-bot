using System;

namespace TelegramBot.Payment
{
    public class User
    {
        public long? Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }

        public override string ToString()
        {
            return Firstname != null ? $"{Firstname} {Lastname}" : $"{Username}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is User user)
            {
                return user.Firstname == Firstname && user.Lastname == Lastname && user.Username == Username;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Firstname, Lastname, Username);
        }
    }
}