namespace Trevo.Core.Model.UserFollower
{
    public class UserFollowDetailsWithUserName :BaseEntity
    {
        public long UserFollowId { get; set; }

        public long FollowerUserId { get; set; }

        public long FollowingUserId { get; set; }

        public string FollowerName { get; set; }

        public string FollowingName { get; set; }
    }
}
