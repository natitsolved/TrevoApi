namespace Trevo.Core.Model.UserFollower
{
   public class UserFollowerDetails :BaseEntity
    {
        public long UserFollowId { get; set; }

        public long FollowerUserId { get; set; }

        public long FollowingUserId { get; set; }
    }
}
