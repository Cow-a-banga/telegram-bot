using System;

namespace DataBase.Models
{
    public class PaymentDto
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public long UserFromId { get; set; }
        public long? UserToId { get; set; }
        public DateTime? PayDate { get; set; }

        public UserDto UserFrom { get; set; }
        public UserDto UserTo { get; set; }
        public string Discriminator { get; set; }
    }
}