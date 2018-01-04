using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.Block;

namespace Trevo.Data.Mapping.BlockMaping
{
  public  class UserBlockMapping : EntityTypeConfiguration<UserBlockDetails>
    {
        public UserBlockMapping()
        {
            this.HasKey(a => a.BlockId);

            this.ToTable("UserBlockDetails");
            this.Property(t => t.BlockId).HasColumnName("BlockId");
            this.Property(t => t.BlockedUserId).HasColumnName("BlockedUserId");
            this.Property(t => t.BlockingTime).HasColumnName("BlockingTime");
            this.Property(t => t.BlockingUserId).HasColumnName("BlockingUserId");
        }
    }
}
