using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.User;

namespace Trevo.Data.Mapping.UserMapping
{
    public  class TrevoUsersMapping : EntityTypeConfiguration<TrevoUsers>
    {
        public TrevoUsersMapping()
        {
            this.HasKey(t => t.User_Id);

            this.ToTable("TrevoUsers");

            //Required fields
            this.Property(t => t.Country_Id).IsRequired();
            this.Property(t => t.CreatedTime).IsRequired();
            this.Property(t => t.LagLevel_ID).IsRequired();



            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Country_Id).HasColumnName("Country_Id");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.DeviceId).HasColumnName("DeviceId").HasMaxLength(255);
            this.Property(t => t.Dob).HasColumnName("Dob").HasMaxLength(255);
            this.Property(t => t.Email_Id).HasColumnName("Email_Id").HasMaxLength(255);
            this.Property(t => t.Gender).HasColumnName("Gender").HasMaxLength(15);
            this.Property(t => t.ImagePath).HasColumnName("ImagePath");
            this.Property(t => t.Interests).HasColumnName("Interests");
            this.Property(t => t.IsVerified).HasColumnName("IsVerified");
            this.Property(t => t.LagLevel_ID).HasColumnName("LagLevel_ID");
            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(255);
            this.Property(t => t.Password).HasColumnName("Password").HasMaxLength(255);
            this.Property(t => t.PasswordHash).HasColumnName("PasswordHash");
            this.Property(t => t.QR_Code).HasColumnName("QR_Code");
            this.Property(t => t.Self_Introduction).HasColumnName("Self_Introduction");
            this.Property(t => t.TravelDestination_CId).HasColumnName("TravelDestination_CId");
            this.Property(t => t.TrevoId).HasColumnName("TrevoId").HasMaxLength(10);
            this.Property(t => t.User_Id).HasColumnName("User_Id");
            this.Property(t => t.ExternalAuthType).HasColumnName("ExternalAuthType");
            this.Property(t => t.ExternalAuthUserId).HasColumnName("ExternalAuthUserId");
        }
    }
}
