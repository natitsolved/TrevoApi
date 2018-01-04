namespace Trevo.Core.Model.Block
{
   public class UserBlockDetails :BaseEntity
    {
        public long BlockId { get; set; }

        public long BlockingUserId { get; set; }

        public long BlockedUserId { get; set; }

        public string BlockingTime { get; set; }
    }
}
