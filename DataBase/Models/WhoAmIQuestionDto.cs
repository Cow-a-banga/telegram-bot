using System;

namespace DataBase.Models
{
    public class WhoAmIQuestionDto
    {
        public long Id { get; set; }
        public long PlayerFromId { get; set; }
        public long? PlayerToId { get; set; }
        public string? Text { get; set; }
        public DateTime? PlayDate { get; set; }

        public UserDto PlayerFrom { get; set; }
        public UserDto PlayerTo{ get; set; }
    }
}