using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.UserUploadDetails;

namespace Trevo.Data.Mapping.UserUploadsMaping
{
    public  class UserUploadsMapping : EntityTypeConfiguration<UserUploads>
    {
        public UserUploadsMapping()
        {
            this.HasKey(t => t.UserUploadsId);

            this.ToTable("UserUploads");
            this.Property(t => t.UploadedAudioPath).HasColumnName("UploadedAudioPath");
            this.Property(t => t.UploadedImagePath).HasColumnName("UploadedImagePath");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserUploadsId).HasColumnName("UserUploadsId");
        }
    }
}
