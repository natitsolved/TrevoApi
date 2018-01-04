using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.User;

namespace Trevo.Data.Mapping.UserMapping
{
    public class UserHobbiesMapping : EntityTypeConfiguration<UserHobbiesDetails>
    {
        public UserHobbiesMapping()
        {
            this.HasKey(t => t.UserHobbiesId);

            this.ToTable("UserHobbies");

            this.Property(t => t.HobbiesId).HasColumnName("HobbiesId");
            this.Property(t => t.UserHobbiesId).HasColumnName("UserHobbiesId");
            this.Property(t => t.User_Id).HasColumnName("User_Id");
        }
    }
}
