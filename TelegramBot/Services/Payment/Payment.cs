using System;
using System.Collections.Generic;
using DataBase.Models;

namespace TelegramBot.Services.Payment
{
    public class Payment: ICloneable
    {
        public decimal? Amount { get; set; }
        public long UserFromId { get; set; }
        public long? UserToId { get; set; }

        public Payment() {}

        public Payment(PaymentDto dto)
        {
            Amount = dto.Amount;
            UserFromId = dto.UserFromId;
            UserToId = dto.UserToId;
        }

        public PaymentDto ToDto()
        {
            return new PaymentDto
            {
                Amount = Amount.Value,
                UserFromId = UserFromId,
                UserToId = UserToId,
            };
        }

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