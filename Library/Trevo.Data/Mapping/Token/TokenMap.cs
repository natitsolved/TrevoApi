using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Token;

namespace Trevo.Data.Mapping.Token
{
    public partial class TokenMap : EntityTypeConfiguration<TokenEntity>
    {
        public TokenMap()
        {
           
            // Primary Key
            this.HasKey(t => t.Token);

            // Properties
            this.Property(t => t.Token)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("AccessToken");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.Device_UID).HasColumnName("Device_UID");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IndividualId).HasColumnName("IndividualId");
            this.Property(t => t.TimeStamp).HasColumnName("TimeStamp");

            //// Relationships
            //this.HasRequired(t => t.ContactType)
            //    .WithMany(t => t.Contacts)
            //    .HasForeignKey(d => d.ContactTypeId);

        }
    }
}
