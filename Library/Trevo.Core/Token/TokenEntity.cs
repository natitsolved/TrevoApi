using System;

namespace Trevo.Core.Token
{
    public class TokenEntity:BaseEntity
    {
        public string Token { get; set; }
        public int Device_UID { get; set; }
        public int UserId { get; set; }
        public int IndividualId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
