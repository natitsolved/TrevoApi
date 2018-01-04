using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.UserFollower;

namespace Trevo.Data.Mapping.UserFollowerMaping
{
    public class UserFollowerMapping : EntityTypeConfiguration<UserFollowerDetails>
    {

        public UserFollowerMapping()
        {
            this.HasKey(a => a.UserFollowId);

            this.ToTable("UserFollowDetails");
            this.Property(t => t.FollowerUserId).HasColumnName("FollowerUserId");
            this.Property(t => t.FollowingUserId).HasColumnName("FollowingUserId");
            this.Property(t => t.UserFollowId).HasColumnName("UserFollowId");
        }
    }
}
