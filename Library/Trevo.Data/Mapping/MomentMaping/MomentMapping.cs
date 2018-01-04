using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.MomentDetails;

namespace Trevo.Data.Mapping.MomentMaping
{
    public  class MomentMapping : EntityTypeConfiguration<Moments>
    {
        public MomentMapping()
        {
            this.HasKey(t => t.MomentId);

            this.ToTable("Moments");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.MomentId).HasColumnName("MomentId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.PosterUserId).HasColumnName("PosterUserId");
            this.Property(t => t.PostingTime).HasColumnName("PostingTime");
            this.Property(t => t.UserUploadedId).HasColumnName("UserUploadedId");
        }
    }
}
