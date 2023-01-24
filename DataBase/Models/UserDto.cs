namespace DataBase.Models
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? BankName { get; set; }

        public override string ToString()
        {
            return $"{Name} {Lastname}";
        }
    }
}