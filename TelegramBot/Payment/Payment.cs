using System;
using System.Collections.Generic;
using DataBase.Models;

namespace TelegramBot.Payment
{
    public class Payment: ICloneable
    {
        public decimal? Amount { get; set; }
        public long UserFromId { get; set; }
        public long? UserToId { get; set; }

        public object Clone()
        {
            return new Payment {Amount = Amount, UserFromId = UserFromId, UserToId = UserToId};
        }

        public T ToDto<T>(Dictionary<long, UserDto> users) where T:PaymentInputDto, new()
        {
            return new()
            {
                Amount = Amount, UserFrom = users[UserFromId], UserTo = UserToId.HasValue ? users[UserToId.Value] : null
            };
        }
    }
}